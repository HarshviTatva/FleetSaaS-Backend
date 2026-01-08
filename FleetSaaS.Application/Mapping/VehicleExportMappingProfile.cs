using AutoMapper;
using FleetSaaS.Application.DTOs.Response;

namespace FleetSaaS.Application.Mapping
{
    public class VehicleExportMappingProfile : Profile
    {
        public VehicleExportMappingProfile() 
        {
            CreateMap<VehicleDTO, VehicleExportDTO>();
        }
    }
}
