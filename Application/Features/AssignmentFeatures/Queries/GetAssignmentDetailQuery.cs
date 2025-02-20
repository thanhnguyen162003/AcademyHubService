using Application.Common.Models.AssignmentModel;
using Application.Common.Models.ZoneModel;
using AutoMapper;
using Domain.Entity;
using Domain.Models.Common;
using Infrastructure;
using Infrastructure.Repositories;
using MediatR;
using Microsoft.AspNetCore.Authentication;
using StackExchange.Redis;
using System.ComponentModel.DataAnnotations;

namespace Application.Features.AssignmentFeatures.Queries
{
   
    public class GetAssignmentDetailQuery : IRequest<AssignmentDetailResponseModel>
    {
        public Guid AssignmentId { get; set; }
    }

    public class GetAssignmentDetailQueryHandler : IRequestHandler<GetAssignmentDetailQuery, AssignmentDetailResponseModel>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        //private readonly IAuthenticationFeature _claimInterface;
        public GetAssignmentDetailQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            //_claimInterface = claimInterface;
        }
        public async Task<AssignmentDetailResponseModel> Handle(GetAssignmentDetailQuery request, CancellationToken cancellationToken)
        {
            var result = await _unitOfWork.AssignmentRepository.GetAssignmentDetail(request.AssignmentId);
            var zone = _mapper.Map<AssignmentDetailResponseModel>(result);
            return zone;

        }
    }
}

