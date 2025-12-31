using AutoMapper;
using FleetSaaS.Application.DTOs.Response;
using FleetSaaS.Domain.Entities;

namespace FleetSaaS.Application.Mapping
{
    public class VehicleDTOMappingProfile : Profile
    {
        public VehicleDTOMappingProfile() {
            CreateMap<Vehicle, VehicleDTO>();
        }
    }
}
