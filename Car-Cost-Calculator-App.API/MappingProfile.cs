using AutoMapper;
using CarCostCalculator_App.EF.Entities;
using DTO = CarCostCalculator_App.Domain.Model;

namespace Car_Cost_Calculator_App.API
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<AnnualData, DTO.AnnualData>()
                .ForMember(dest => dest.TotalKilometers, opt => opt.MapFrom(src => src.MonthlyData.SelectMany(m => m.KilometerEntries).Sum(k => k.Kilometers)))
                .ForMember(dest => dest.TotalCost, opt => opt.MapFrom(src => src.MonthlyData.SelectMany(m => m.CostEntries).Sum(c => c.Price)))
                .ReverseMap();

            CreateMap<Category, DTO.Category>()
               .ReverseMap();

            CreateMap<CostEntry, DTO.CostEntry>()
                .ReverseMap();

            CreateMap<KilometerEntry, DTO.KilometerEntry>()
                .ReverseMap();

            CreateMap<MonthlyData, DTO.MonthlyData>()
                .ForMember(dest => dest.TotalKilometers, opt => opt.MapFrom(src => src.KilometerEntries.Sum(k => k.Kilometers)))
                .ForMember(dest => dest.TotalCost, opt => opt.MapFrom(src => src.CostEntries.Sum(c => c.Price)))
                .ReverseMap();


        }
    }
}
