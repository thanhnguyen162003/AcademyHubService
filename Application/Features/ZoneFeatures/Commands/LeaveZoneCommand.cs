using Application.Messages;
using Application.Services.Authentication;
using Domain.Models.Common;
using Infrastructure.Repositories;
using MediatR;
using System.Net;

namespace Application.Features.ZoneFeatures.Commands
{
    public class LeaveZoneCommand : IRequest<APIResponse>
    {
        public Guid ZoneId { get; set; }
    }

    public class LeaveZoneCommandHandler : IRequestHandler<LeaveZoneCommand, APIResponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAuthenticationService _authenticationService;

        public LeaveZoneCommandHandler(IUnitOfWork unitOfWork, IAuthenticationService authenticationService)
        {
            _unitOfWork = unitOfWork;
            _authenticationService = authenticationService;
        }

        public async Task<APIResponse> Handle(LeaveZoneCommand request, CancellationToken cancellationToken)
        {
            var zone = await _unitOfWork.ZoneRepository.GetById(request.ZoneId);

            if(zone == null)
            {
                return new APIResponse()
                {
                    Status = HttpStatusCode.NotFound,
                    Message = MessageCommon.NotFound
                };
            }

            var userId = _authenticationService.User.UserId;
            if(!zone.CreatedBy.Equals(userId))
            {
                return new APIResponse()
                {
                    Status = HttpStatusCode.Forbidden,
                    Message = MessageCommon.Forbidden
                };
            }

            zone.DeletedAt = DateTime.Now;
            await _unitOfWork.ZoneRepository.Update(zone);

            if(await _unitOfWork.SaveChangesAsync())
            {
                return new APIResponse()
                {
                    Status = HttpStatusCode.OK,
                    Message = MessageCommon.DeleteSuccessfully,
                    Data = zone.Id
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
