using AutoMapper;
using FleetSaaS.Application.DTOs.Request;
using FleetSaaS.Application.DTOs.Response;
using FleetSaaS.Application.Interfaces.IRepositories;
using FleetSaaS.Application.Interfaces.IServices;

namespace FleetSaaS.Application.Services
{
    public class VehicleService(IVehicleRepository vehicleRepository,IMapper mapper) : IVehicleService
    {
        public async Task<VehicleResponse> GetAllVehicles(PagedRequest request)
        {
            return await vehicleRepository.GetAllVehicles(request);
        }

        public async Task DeleteVehicle(Guid Id)
        {
            await vehicleRepository.DeleteVehicle(Id);
        }
       
    }
}
