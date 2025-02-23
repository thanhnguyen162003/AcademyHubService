using Application.Common.Messages;
using Application.Common.Models.TestContent;
using Application.Messages;
using Application.Services.Authentication;
using AutoMapper;
using Domain.Entity;
using Domain.Models.Common;
using Infrastructure.Repositories;
using MediatR;
using System.Net;
using System.Text.Json.Serialization;

namespace Application.Features.AssignmentFeatures.Commands
{
    public class CreateSubmissionCommand : IRequest<APIResponse>
    {
        [JsonIgnore]
        public Guid AssignmentId { get; set; }
        public List<TestContentSubmissionResponseModel> Answer { get; set; }


    }
    public class CreateSubmissionCommandHandler : IRequestHandler<CreateSubmissionCommand, APIResponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IAuthenticationService _authenticationService;

        public CreateSubmissionCommandHandler(IUnitOfWork unitOfWork, IMapper mapper, IAuthenticationService authenticationService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _authenticationService = authenticationService;
        }

        public async Task<APIResponse> Handle(CreateSubmissionCommand request, CancellationToken cancellationToken)
        {
            // Check assignment exists
            var assignment = await _unitOfWork.AssignmentRepository.GetById(request.AssignmentId);

            if (assignment == null)
            {
                return new APIResponse()
                {
                    Status = HttpStatusCode.NotFound,
                    Message = MessageCommon.NotFound,
                    Data = request.AssignmentId
                };
            }

            // Check role of user performing action
            var zone = await _unitOfWork.ZoneRepository.GetById(assignment.ZoneId);
            var userId = _authenticationService.User.UserId;
            var membership = await _unitOfWork.ZoneMembershipRepository.GetMembership(userId, zone.Id);
            if (membership is null)
            {
                return new APIResponse()
                {
                    Status = HttpStatusCode.Forbidden,
                    Message = MessageCommon.Forbidden
                };
            }
            var listTest = _mapper.Map<List<TestContent>>(request.Answer);
            var wrongAnswer = await _unitOfWork.TestContentRepository.SubmitTest(listTest);
            //test
            Submission submission = new Submission()
            {
                Id = Guid.NewGuid(),
                AssignmentId = assignment.Id,
                CreatedAt = DateTime.Now,
                MemberId = membership.Id,
                Score = ((double)assignment.TotalQuestion - (double)wrongAnswer.Count) / (double)assignment.TotalQuestion * 10,

            };

            await _unitOfWork.SubmissionRepository.Add(submission);
            var result = await _unitOfWork.SaveChangesAsync();
            if (result)
            {
                return new APIResponse()
                {
                    Status = HttpStatusCode.OK,
                    Message = MessageZone.AssignmentCreatedSuccess,
                    Data = submission.Score
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
