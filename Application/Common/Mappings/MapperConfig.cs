using Application.Features.AssignmentFeatures.Commands;
using Application.Features.ZoneFeatures.Commands;
using AutoMapper;
using Domain.Entity;

namespace Application.Common.Mappings
{
    public class MapperConfig : Profile
    {

        public MapperConfig()
        {
            Config();
        }

        private void Config()
        {
            CreateMap<CreateZoneCommand, Zone>()
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => DateTime.Now))
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => Guid.NewGuid()))
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

            CreateMap<UpdateZoneCommand, Zone>()
                .ForMember(dest => dest.CreatedBy, opt => opt.MapFrom(src => src.TransferOwner))
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

            CreateMap<CreateAssignmentCommand, Assignment>()
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => DateTime.Now))
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => Guid.NewGuid()))
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
        }

    }
}
