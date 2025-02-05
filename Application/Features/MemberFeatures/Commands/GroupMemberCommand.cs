using Application.Common.Messages;
using Domain.Entity;
using Domain.Models.Common;
using Infrastructure.Repositories;
using MediatR;
using System.Net;

namespace Application.Features.MemberFeatures.Commands
{
    public class GroupMemberCommand : IRequest<APIResponse>
    {
        public int NumberGroup { get; set; }
        public Guid ZoneId { get; set; }
    }

    public class GroupMemberCommandHandler : IRequestHandler<GroupMemberCommand, APIResponse>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GroupMemberCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<APIResponse> Handle(GroupMemberCommand request, CancellationToken cancellationToken)
        {
            // Get all members of the group
            var members = await _unitOfWork.ZoneMembershipRepository.GetAll(
                    m => m.GroupId.Equals(request.ZoneId)
                );

            // Check if the number of members is enough to divide into groups
            var membersCount = members.Count(); // Count the number of members
            var divideMember = membersCount / request.NumberGroup;  // Divide the number of members by the number of groups

            if (divideMember <= 0)
            {
                return new APIResponse()
                {
                    Status = HttpStatusCode.BadRequest,
                    Message = MessageZone.MemberNotEnough
                };
            }

            // Divide the members into groups
            var random = new Random();
            var groupMembers = new List<List<ZoneMembership>>();
            groupMembers = members
                .OrderBy(x => random.Next())
                .Select((x, i) => new { Index = i, Value = x })
                .GroupBy(x => x.Index / divideMember)
                .Select(x => x.Select(v => v.Value).ToList())
                .ToList();

            return new APIResponse()
            {
                Status = HttpStatusCode.OK,
                Data = groupMembers
            };
        }
    }

}
