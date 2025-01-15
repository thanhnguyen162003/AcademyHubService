using Application.Common.Helper;
using Application.Messages;
using Application.Services.Authentication;
using AutoMapper;
using Domain.Models.Common;
using FluentValidation;
using Infrastructure.Repositories;
using MediatR;
using System.Net;
using System.Text.Json.Serialization;

namespace Application.Features.ZoneFeatures.Commands
{
    public class UpdateZoneCommand : IRequest<APIResponse>
    {
        [JsonIgnore]
        public Guid Id { get; set; }

        public string Name { get; set; } = null!;

        public string Description { get; set; } = null!;

        public string? LogoUrl { get; set; }

        public string? BannerUrl { get; set; }
    }

    public class UpdateZoneCommandValidator : AbstractValidator<UpdateZoneCommand>
    {
        public UpdateZoneCommandValidator()
        {
            RuleFor(p => p.LogoUrl)
                .Must(v => (v == null || v.IsValidUrl())).WithMessage("{PropertyName} must be a valid URL.");

            RuleFor(p => p.BannerUrl)
                .Must(v => (v == null || v.IsValidUrl())).WithMessage("{PropertyName} must be a valid URL.");
        }
    }

    public class UpdateZoneCommandHandler : IRequestHandler<UpdateZoneCommand, APIResponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAuthenticationService _authenticationService;
        private readonly IMapper _mapper;

        public UpdateZoneCommandHandler(IUnitOfWork unitOfWork, IMapper mapper,
                IAuthenticationService authenticationService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _authenticationService = authenticationService;
        }

        public async Task<APIResponse> Handle(UpdateZoneCommand request, CancellationToken cancellationToken)
        {
            // Check zone exists
            var zone = await _unitOfWork.ZoneRepository.GetById(request.Id);

            if (zone == null)
            {
                return new APIResponse()
                {
                    Status = HttpStatusCode.NotFound,
                    Message = MessageCommon.NotFound,
                    Data = request.Id
                };
            }

            // Check role of user performing action
            var userId = _authenticationService.GetUserId();
            if(!await _unitOfWork.ZoneMembershipRepository.IsAdminZone(userId, zone.Id))
            {
                return new APIResponse()
                {
                    Status = HttpStatusCode.Forbidden,
                    Message = MessageCommon.Forbidden
                };
            }

            // Update zone
            zone = _mapper.Map(request, zone);
            await _unitOfWork.ZoneRepository.Update(zone);

            if (await _unitOfWork.SaveChangesAsync())
            {
                return new APIResponse()
                {
                    Status = HttpStatusCode.OK,
                    Message = MessageCommon.UpdateSuccesfully,
                    Data = zone.Id
                };
            }

            return new APIResponse()
            {
                Status = HttpStatusCode.InternalServerError,
                Message = MessageCommon.UpdateFailed
            };

        }

    }
}
