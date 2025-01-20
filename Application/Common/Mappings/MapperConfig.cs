using Application.Features.ZoneFeatures.Commands;
using Application.Services.Authentication;
using AutoMapper;
using Domain.Entity;

namespace Application.Common.Mappings
{
    public class MapperConfig : Profile
    {
        private readonly IAuthenticationService _authenticationService;

        public MapperConfig(IAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
            Config();
        }

        private void Config()
        {
            CreateMap<CreateZoneCommand, Zone>()
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => DateTime.Now))
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => Guid.NewGuid()))
                .ForMember(dest => dest.CreatedBy, opt => opt.MapFrom(src => _authenticationService.User.UserId))
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

            CreateMap<UpdateZoneCommand, Zone>()
                .ForMember(dest => dest.CreatedBy, opt => opt.MapFrom(src => src.TransferOwner))
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
        }
    }
}
