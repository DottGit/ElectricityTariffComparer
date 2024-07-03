using AutoMapper;
using static ElectricityTariffComparer.Models.Dto;
using static ElectricityTariffComparer.Models.TariffProvider;

namespace ElectricityTariffComparer.Mapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<TariffDto, BasicElectricityTariff>()
                .ForMember(dest => dest.BaseCost, opt => opt.MapFrom(src => src.BaseCost))
                .ForMember(dest => dest.AdditionalKwhCost, opt => opt.MapFrom(src => src.AdditionalKwhCost));

            CreateMap<TariffDto, PackagedTariff>()
                .ForMember(dest => dest.BaseCost, opt => opt.MapFrom(src => src.BaseCost))
                .ForMember(dest => dest.IncludedKwh, opt => opt.MapFrom(src => src.IncludedKwh))
                .ForMember(dest => dest.AdditionalKwhCost, opt => opt.MapFrom(src => src.AdditionalKwhCost));
        }
    }
}
