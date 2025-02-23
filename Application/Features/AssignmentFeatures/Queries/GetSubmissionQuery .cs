using Application.Common.Models.SubmissionContent;
using AutoMapper;
using Domain.Models.Common;
using Infrastructure.Repositories;
using MediatR;
using Application.Services.Authentication;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Application.Features.AssignmentFeatures.Queries
{
    public record GetSubmissionQuery : IRequest<PagedList<SubmissionResponseModel>>
    {
        [JsonIgnore]
        public Guid AssignmentId { get; set; }

        [Required]
        public int PageSize { get; set; }
        [Required]
        public int PageNumber { get; set; }

        [Required]
        public bool IsAscending { get; set; }
    }

    public class GetSubmissionQueryHandler : IRequestHandler<GetSubmissionQuery, PagedList<SubmissionResponseModel>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IAuthenticationService _authenticationService;
        public GetSubmissionQueryHandler(IUnitOfWork unitOfWork, IMapper mapper, IAuthenticationService authenticationService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _authenticationService = authenticationService;
        }

        public async Task<PagedList<SubmissionResponseModel>> Handle(GetSubmissionQuery request, CancellationToken cancellationToken)
        {
            // Check assignment exists
            var assignment = await _unitOfWork.AssignmentRepository.GetById(request.AssignmentId);

            if (assignment == null)
            {
                return new PagedList<SubmissionResponseModel>(new List<SubmissionResponseModel>(), 0, 0, 0);
            }
            var zone = await _unitOfWork.ZoneRepository.GetById(assignment.ZoneId);
            var userId = _authenticationService.User.UserId;
            var membership = await _unitOfWork.ZoneMembershipRepository.GetMembership(userId, zone.Id);
            var result = await _unitOfWork.SubmissionRepository.GetSubmission(request.AssignmentId, membership.Id ,request.PageNumber, request.PageSize, request.IsAscending);
            if (!result.Any())
            {
                return new PagedList<SubmissionResponseModel>(new List<SubmissionResponseModel>(), 0, 0, 0);
            }
            return _mapper.Map<PagedList<SubmissionResponseModel>>(result);
        }
    }
}