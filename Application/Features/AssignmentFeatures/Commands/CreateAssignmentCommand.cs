using Application.Common.Messages;
using Application.Services.Authentication;
using AutoMapper;
using Domain.Entity;
using Domain.Models.Common;
using Infrastructure.Repositories;
using MediatR;
using System.Net;

namespace Application.Features.AssignmentFeatures.Commands
{
    public class CreateAssignmentCommand : IRequest<APIResponse>
    {
        public Guid ZoneId { get; set; }

        public string? Type { get; set; }

        public string? Title { get; set; }

        public string? Noticed { get; set; }

        public int? TotalQuestion { get; set; }

        public int? TotalTime { get; set; }

        public DateTime? AvailableAt { get; set; }

        public DateTime? DueAt { get; set; }

        public DateTime? LockedAt { get; set; }

        public bool? Published { get; set; }
    }

    public class CreateAssignmentCommandHandler : IRequestHandler<CreateAssignmentCommand, APIResponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IAuthenticationService _authenticationService;

        public CreateAssignmentCommandHandler(IUnitOfWork unitOfWork, IMapper mapper, IAuthenticationService authenticationService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _authenticationService = authenticationService;
        }

        public async Task<APIResponse> Handle(CreateAssignmentCommand request, CancellationToken cancellationToken)
        {
            var assignment = _mapper.Map<Assignment>(request);
            assignment.CreatedBy = _authenticationService.User.UserId;

            await _unitOfWork.AssignmentRepository.Add(assignment);

            if(await _unitOfWork.SaveChangesAsync())
            {
                return new APIResponse()
                {
                    Status = HttpStatusCode.OK,
                    Message = MessageZone.AssignmentCreatedSuccess,
                    Data = assignment.ZoneId
                };
            }

            return new APIResponse()
            {
                Status = HttpStatusCode.InternalServerError,
                Message = MessageZone.AssignmentCreatedFailed
            };

        }
    }
}
