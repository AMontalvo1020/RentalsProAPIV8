using AutoMapper;
using RentalsProAPIV8.Client.DataTransferObjects;
using RentalsProAPIV8.Models;

namespace RentalsProAPIV8.Infrastructure.Profiles
{
    public class PropertyProfile : Profile
    {
        public PropertyProfile()
        {
            CreateMap<Property, PropertyDTO>().ForMember(dest => dest.Owner, opt => opt.MapFrom(src => src.Owner))
                                              .ForMember(dest => dest.Company, opt => opt.MapFrom(src => src.Company))
                                              .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status))
                                              .ForMember(dest => dest.Type, opt => opt.MapFrom(src => src.Type))
                                              .ForMember(dest => dest.PaymentStatus, opt => opt.MapFrom(src => src.PaymentStatus));

            CreateMap<PropertyDTO, Property>();
        }
    }
}
