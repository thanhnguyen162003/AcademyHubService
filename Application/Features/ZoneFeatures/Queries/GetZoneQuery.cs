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

namespace Application.Features.ZoneFeatures.Queries
{
   
    public class GetZoneQuery : IRequest<PagedList<ZoneResponseModel>>
    {
        public string? Search { get; set; }
        [Required]
        public int PageSize { get; set; }
        [Required]
        public int PageNumber { get; set; }
        

    }

    public class GetZoneQueryHandler: IRequestHandler<GetZoneQuery, PagedList<ZoneResponseModel>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        //private readonly IAuthenticationFeature _claimInterface;
        public GetZoneQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            //_claimInterface = claimInterface;
        }
        public async Task<PagedList<ZoneResponseModel>> Handle(GetZoneQuery request, CancellationToken cancellationToken)
        {
            var result = await _unitOfWork.ZoneRepository.GetZoneForStudent(request.PageNumber, request.PageSize, request.Search);
            if (!result.Any()) 
            {
                return new PagedList<ZoneResponseModel>(new List<ZoneResponseModel>(), 0, 0, 0);
            }
            var zone = _mapper.Map<PagedList<ZoneResponseModel>>(result);
            return zone;

        }
    }
}

