using Application.Common.Models.OtherModel;
using Application.Common.Models.ZoneMembershipModel;
using AutoMapper;
using Domain.Entity;
using Domain.Models.Common;
using Infrastructure.Repositories;
using MediatR;
using static Domain.Enums.ZoneEnums;

namespace Application.Features.MemberFeatures.Queries
{
    public class GetMemberQuery : IRequest<object>
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public MemberQueryType Type { get; set; }
        public Guid ZoneId { get; set; }
    }

    public class GetMemberQueryHandler : IRequestHandler<GetMemberQuery, object>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly AcademicServiceRpc.AcademicServiceRpcClient _academicServiceRpcClient;
        private readonly IMapper _mapper;

        public GetMemberQueryHandler(IUnitOfWork unitOfWork, IMapper mapper,
            AcademicServiceRpc.AcademicServiceRpcClient academicServiceRpcClient)
        {
            _unitOfWork = unitOfWork;
            _academicServiceRpcClient = academicServiceRpcClient;
            _mapper = mapper;
        }

        public async Task<object> Handle(GetMemberQuery request, CancellationToken cancellationToken)
        {
            if (request.Type == MemberQueryType.Member)
            {
                return await GetMembers(request.ZoneId, request.PageSize, request.PageNumber);
            } else if (request.Type == MemberQueryType.Pending)
            {
                return await GetPendingMembers(request.ZoneId, request.PageSize, request.PageNumber);
            }

            return new PagedList<object>();
        }

        private async Task<PagedList<MemberZoneResponseModel>> GetMembers(Guid zoneId, int pageSize, int pageNumber)
        {
            var members = await _unitOfWork.ZoneMembershipRepository.GetAll(m => m.ZoneId.Equals(zoneId), pageNumber, pageSize);

            GetAcademicUserRequest requestRpc = new();
            requestRpc.Emails.AddRange(members.Select(m => m.Email));

            var memberResponse = _mapper.Map<PagedList<MemberZoneResponseModel>>(members);

            var response = await _academicServiceRpcClient.GetUsersAsync(requestRpc);

            foreach (var member in memberResponse)
            {
                member.User = _mapper.Map<UserModel>(response.Objects.FirstOrDefault(r => r.Email.Equals(member.Email)));
            }

            return memberResponse;
        }

        private async Task<PagedList<MemberPendingResponseModel>> GetPendingMembers(Guid zoneId, int pageSize, int pageNumber)
        {
            var members = await _unitOfWork.PendingZoneInviteRepository.GetAll(m => m.ZoneId.Equals(zoneId), pageNumber, pageSize);

            GetAcademicUserRequest requestRpc = new();
            requestRpc.Emails.AddRange(members.Select(m => m.Email));

            var memberResponse = _mapper.Map<PagedList<MemberPendingResponseModel>>(members);

            var response = await _academicServiceRpcClient.GetUsersAsync(requestRpc);

            foreach (var member in memberResponse)
            {
                member.User = _mapper.Map<UserModel>(response.Objects.FirstOrDefault(r => r.Email.Equals(member.Email)));
            }

            return memberResponse;
        }
    }

}
