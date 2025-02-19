using Application.Common.Messages;
using Application.Messages;
using Application.Services.Authentication;
using AutoMapper;
using Domain.Entity;
using Domain.Models.Common;
using FluentValidation;
using Infrastructure.Repositories;
using MediatR;
using System.Net;
using System.Text.Json.Serialization;

namespace Application.Features.AssignmentFeatures.Commands
{
    public class UpdateAssignmentCommand : IRequest<APIResponse>
    {
        [JsonIgnore]
        public Guid Id { get; set; }

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
    public class UpdateAssignmentCommandValidator : AbstractValidator<UpdateAssignmentCommand>
    {
        public UpdateAssignmentCommandValidator()
        {
            RuleFor(p => p.Type)
                .NotNull().WithMessage("{PropertyName} is required.")
                .NotEmpty().WithMessage("{PropertyName} is required.");

            RuleFor(p => p.Title)
                .NotNull().WithMessage("{PropertyName} is required.")
                .NotEmpty().WithMessage("{PropertyName} is required.");

            RuleFor(p => p.AvailableAt)
                .NotNull().WithMessage("{PropertyName} is required.")
                .NotEmpty().WithMessage("{PropertyName} is required.")
                .GreaterThan(DateTime.Now).WithMessage("{PropertyName} must be after now."); 

            RuleFor(p => p.DueAt)
                .NotNull().WithMessage("{PropertyName} is required.")
                .GreaterThan(p => p.AvailableAt).WithMessage("{PropertyName} must be after AvailableAt."); 

            RuleFor(p => p.LockedAt)
                .NotNull().WithMessage("{PropertyName} is required.")
                .GreaterThan(p => p.DueAt).WithMessage("{PropertyName} must be after DueAt."); 
        }
    }
    public class UpdateAssignmentCommandHandler : IRequestHandler<UpdateAssignmentCommand, APIResponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IAuthenticationService _authenticationService;

        public UpdateAssignmentCommandHandler(IUnitOfWork unitOfWork, IMapper mapper, IAuthenticationService authenticationService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _authenticationService = authenticationService;
        }

        public async Task<APIResponse> Handle(UpdateAssignmentCommand request, CancellationToken cancellationToken)
        {
            // Check assignment exists
            var assignment = await _unitOfWork.AssignmentRepository.GetById(request.Id);

            if (assignment == null)
            {
                return new APIResponse()
                {
                    Status = HttpStatusCode.NotFound,
                    Message = MessageCommon.NotFound,
                    Data = request.Id
                };
            }

            var zone = await _unitOfWork.ZoneRepository.GetById(assignment.ZoneId);
            // Check role of user performing action
            var userId = _authenticationService.User.UserId;
            if (!await _unitOfWork.ZoneMembershipRepository.IsTeacherInZone(userId, assignment.ZoneId) && !zone.CreatedBy.Equals(userId))
            {
                return new APIResponse()
                {
                    Status = HttpStatusCode.Forbidden,
                    Message = MessageCommon.Forbidden
                };
            }

            assignment = _mapper.Map(request, assignment);
            assignment.UpdatedAt = DateTime.Now;

            await _unitOfWork.AssignmentRepository.Update(assignment);

            var result = await _unitOfWork.SaveChangesAsync();
            if (result)
            {
                return new APIResponse()
                {
                    Status = HttpStatusCode.OK,
                    Message = MessageCommon.UpdateSuccesfully,
                    Data = assignment.ZoneId
                };
            }

            return new APIResponse()
            {
                Status = HttpStatusCode.InternalServerError,
                Message = MessageCommon.UpdateFailed,
            };

        }
    }
}
