using AutoMapper;
using CurrencyConverter.DTOs;
using CurrencyConverter.Entities;

namespace CurrencyConverter.Mappers
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile() {
            CreateMap<ConversionRequestDTO, ConversionLog>();
            CreateMap<ConversionLog, ConversionResponseDTO>()
                .ForMember(dest => dest.Rate, opt => opt.Ignore());


            CreateMap<TopConversionReport, TopConversionReportDTO>();
        }
    }
}
