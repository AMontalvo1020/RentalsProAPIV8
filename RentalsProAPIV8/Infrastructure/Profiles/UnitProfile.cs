using AutoMapper;
using RentalsProAPIV8.Client.DataTransferObjects;
using RentalsProAPIV8.Models;

namespace RentalsProAPIV8.Infrastructure.Profiles
{
    public class UnitProfile : Profile
    {
        public UnitProfile()
        {
            CreateMap<Unit, UnitDTO>().ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status))
                                      .ForMember(dest => dest.PaymentStatus, opt => opt.MapFrom(src => src.PaymentStatus));

            CreateMap<UnitDTO, Unit>();
        }
    }
}
