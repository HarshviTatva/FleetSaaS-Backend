using AutoMapper;
using FleetSaaS.Application.DTOs.Request;
using FleetSaaS.Domain.Entities;

namespace FleetSaaS.Application.Mapping
{
    public class VehicleMapping : Profile
    {
        public VehicleMapping() {
            CreateMap<VehicleRequest, Vehicle>()
         .ForMember(x => x.Make, opt => opt.MapFrom(src => src.Make))
         .ForMember(x => x.Model, opt => opt.MapFrom(src => src.Model))
         .ForMember(x => x.Year, opt => opt.MapFrom(src => src.Year))
         .ForMember(x => x.LicensePlate, opt => opt.MapFrom(src => src.LicensePlate))
         .ForMember(x => x.InsuranceExpiryDate, opt => opt.MapFrom(src => src.InsuranceExpiryDate))
         .ForMember(x => x.Vin, opt => opt.MapFrom(src => src.Vin))
         .ForMember(x => x.IsActive, opt => opt.MapFrom(src => src.IsActive))
         .ForMember(x => x.CompanyId, opt => opt.MapFrom(src => src.CompanyId));
        }
    }
}
