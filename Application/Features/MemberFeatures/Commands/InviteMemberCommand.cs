using Application.Common.Helper;
using Application.Common.Messages;
using Application.Common.Models.ProduceModels;
using Application.Messages;
using Application.Services.Authentication;
using Application.Services.KafkaService.Producer;
using Domain.Constants;
using Domain.Constants.Services;
using Domain.Entity;
using Domain.Enums;
using Domain.Models.Common;
using FluentValidation;
using Infrastructure.Repositories;
using MediatR;
using System.Net;
using static Domain.Enums.ZoneEnums;

namespace Application.Features.MemberFeatures.Commands
{
    public class InviteMemberCommand : IRequest<APIResponse>
    {
        public string? Email { get; set; }
        public string? Type { get; set; }
        public Guid? ZoneId { get; set; }
    }

    public class InviteMemberCommandValidator : AbstractValidator<InviteMemberCommand>
    {
        public InviteMemberCommandValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required")
                .NotNull().WithMessage("Email is required")
                .EmailAddress().WithMessage("Email is not valid");

            RuleFor(x => x.Type)
                .NotEmpty().WithMessage("Type is required")
                .NotNull().WithMessage("Type is required")
                .Must(x => x!.IsInEnum<ZoneMembershipType, string>());

            RuleFor(x => x.ZoneId)
                .NotEmpty().WithMessage("ZoneId is required")
                .NotNull().WithMessage("ZoneId is required");
        }
    }

    public class InviteMemberCommandHandler : IRequestHandler<InviteMemberCommand, APIResponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IProducerService _producerService;
        private readonly IAuthenticationService _authenticationService;

        public InviteMemberCommandHandler(IUnitOfWork unitOfWork, IProducerService producerService, IAuthenticationService authenticationService)
        {
            _unitOfWork = unitOfWork;
            _producerService = producerService;
            _authenticationService = authenticationService;
        }

        public async Task<APIResponse> Handle(InviteMemberCommand request, CancellationToken cancellationToken)
        {
            // Check if zone exists
            var zone = await _unitOfWork.ZoneRepository.GetById(request.ZoneId!);

            if (zone == null)
            {
                return new APIResponse
                {
                    Status = HttpStatusCode.NotFound,
                    Message = MessageCommon.NotFound
                };
            }

            // Check if user is already a member
            var isMember = await _unitOfWork.ZoneMembershipRepository.IsMembership(request.Email!, (Guid)request.ZoneId!);
            if(isMember)
            {
                return new APIResponse()
                {
                    Status = HttpStatusCode.BadRequest,
                    Message = MessageZone.MemberExists
                };
            }

            // Check if user is already invited
            var invite = await _unitOfWork.PendingZoneInviteRepository.GetBy(e =>
                e.ZoneId.Equals(request.ZoneId) && e.Email.Equals(request.Email)
            );

            var userId = _authenticationService.User.UserId;

            if(invite != null)
            {
                // update invite if exists
                if (DateTime.Now < invite.ExpiredAt)
                {
                    return new APIResponse()
                    {
                        Status = HttpStatusCode.BadRequest,
                        Message = MessageZone.CannotInviteAfter24h
                    };
                }
                invite.Type = request.Type!;
                invite.InviteBy = userId;
                invite.ExpiredAt = DateTime.Now.AddHours(24);
                invite.UpdatedAt = DateTime.Now;
                await _unitOfWork.PendingZoneInviteRepository.Update(invite);
            } else
            {
                // create pending invite if not exists
                await _unitOfWork.PendingZoneInviteRepository.Add(new PendingZoneInvite()
                {
                    Email = request.Email!,
                    Type = request.Type!,
                    CreatedAt = DateTime.Now,
                    ZoneId = request.ZoneId!,
                    InviteBy = userId,
                    ExpiredAt = DateTime.Now.AddHours(24)
                });
            }

            if (await _unitOfWork.SaveChangesAsync())
            {
                // Send mail invite
                var result = await _producerService.ProduceObjectWithKeyAsync(TopicKafkaConstaints.MailZoneCreated, userId.ToString(), new MailModel()
                {
                    MailType = MailSendType.InviteMember,
                    MailInviteMemberModel = new MailInviteMemberModel()
                    {
                        CreatedBy = (Guid)zone.CreatedBy!,
                        Email = request.Email!,
                        LogoUrl = zone.LogoUrl!,
                        ZoneName = zone.Name!,
                        CreatedAt = DateTime.Now,
                        Type = request.Type!,
                        Description = zone.Description!,
                        BannerUrl = zone.BannerUrl!,
                        AcceptLink = $"{UrlConstant.ClientUrl}/zone/{zone.Id}/reply/accept",
                        RejectLink = $"{UrlConstant.ClientUrl}/zone/{zone.Id}/reply/reject"
                    }
                });

                if (result)
                {
                    return new APIResponse()
                    {
                        Status = HttpStatusCode.OK,
                        Message = MessageZone.InviteMemberSuccess,
                        Data = zone.Id
                    };

                }
            }

            return new APIResponse()
            {
                Status = HttpStatusCode.InternalServerError,
                Message = MessageCommon.ServerError
            };

        }
    }

}
