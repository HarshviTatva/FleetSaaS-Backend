using AutoMapper;
using FleetSaaS.Application.DTOs.Request;
using FleetSaaS.Domain.Entities;

namespace FleetSaaS.Application.Mapping
{
    public class VehicleAssignmentMapping : Profile
    {
        public VehicleAssignmentMapping()
        {
            CreateMap<AssignVehicleRequest, VehicleAssignment>()
              .ForMember(x => x.Id, opt => opt.MapFrom(src => src.Id))
              .ForMember(x => x.VehicleId, opt => opt.MapFrom(src => src.VehicleId))
              .ForMember(x => x.DriverId, opt => opt.MapFrom(src => src.DriverId))
              .ForMember(x => x.CompanyId, opt => opt.MapFrom(src => src.CompanyId))
              .ForMember(x => x.IsActive, opt => opt.MapFrom(IsActive => true));
        }
    }
}
