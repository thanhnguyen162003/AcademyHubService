using Application.Common.Messages;
using Application.Messages;
using Application.Services.Authentication;
using Domain.Entity;
using Domain.Models.Common;
using Infrastructure.Repositories;
using MediatR;
using System.Net;
using static Domain.Enums.ZoneEnums;

namespace Application.Features.MemberFeatures.Commands
{
    public class ReplyInviteZoneCommand : IRequest<APIResponse>
    {
        public Guid ZoneId { get; set; }
        public RelyInvite RelyInvite { get; set; }
    }

    public class ReplyInviteZoneCommandHandler : IRequestHandler<ReplyInviteZoneCommand, APIResponse>
    {   
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAuthenticationService _authenticationService;

        public ReplyInviteZoneCommandHandler(IUnitOfWork unitOfWork, IAuthenticationService authenticationService)
        {
            _unitOfWork = unitOfWork;
            _authenticationService = authenticationService;
        }

        public async Task<APIResponse> Handle(ReplyInviteZoneCommand request, CancellationToken cancellationToken)
        {
            var userId = _authenticationService.User.UserId;
            var email = _authenticationService.User.Email;
            var pendingZoneInvite = await _unitOfWork.PendingZoneInviteRepository.GetBy(x => x.ZoneId.Equals(request.ZoneId) && 
                                                        x.Email.Equals(email));

            if(pendingZoneInvite == null)
            {
                return new APIResponse()
                {
                    Status = HttpStatusCode.NotFound,
                    Message = MessageZone.InviteIsNotFound
                };
            }

            if(request.RelyInvite == RelyInvite.Accept)
            {
                await _unitOfWork.ZoneMembershipRepository.Add(new ZoneMembership()
                {
                    Email = email,
                    Type = request.RelyInvite.ToString(),
                    ZoneId = request.ZoneId,
                    CreatedAt = DateTime.Now,
                    UserId = userId,
                    InviteBy = pendingZoneInvite.InviteBy
                });
            }

            await _unitOfWork.PendingZoneInviteRepository.Delete(pendingZoneInvite);
            
            if(await _unitOfWork.SaveChangesAsync())
            {
                return new APIResponse()
                {
                    Status = HttpStatusCode.OK,
                    Message = request.RelyInvite == RelyInvite.Accept ? MessageZone.JoinZoneSuccess : MessageZone.RejectInviteSuccess,
                    Data = request.ZoneId
                };
            }

            return new APIResponse()
            {
                Status = HttpStatusCode.InternalServerError,
                Message = MessageCommon.ServerError
            };

        }
    }

}
