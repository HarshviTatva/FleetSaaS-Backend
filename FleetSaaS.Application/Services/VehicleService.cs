using AutoMapper;
using FleetSaaS.Application.DTOs.Request;
using FleetSaaS.Application.DTOs.Response;
using FleetSaaS.Application.Interfaces.IRepositories;
using FleetSaaS.Application.Interfaces.IServices;
using FleetSaaS.Domain.Common.Messages;
using FleetSaaS.Domain.Entities;
using FleetSaaS.Domain.Exceptions;
using FleetSaaS.Domain.Helper;
using FleetSaaS.Infrastructure.Common;

namespace FleetSaaS.Application.Services
{
    public class VehicleService(IVehicleRepository _vehicleRepository,IMapper _mapper) : IVehicleService
    {
        public async Task<VehicleResponse> GetAllVehicles(PagedRequest request)
        {
            return await _vehicleRepository.GetAllVehicles(request);
        }

        public async Task DeleteVehicle(Guid Id)
        {
            await _vehicleRepository.DeleteVehicle(Id);
        }

        public async Task<Guid> AddEditVehicle(VehicleRequest vehicleRequest)
        {
            Vehicle vehicle = _mapper.Map<Vehicle>(vehicleRequest);

            if (await _vehicleRepository.ExistsByVinAsync(vehicleRequest.Vin,vehicleRequest.Id))
                throw new ConflictException(field: Fields.Vin, message: VehicleMessages.VIN_EXISTS);

            if (await _vehicleRepository.ExistsByLicensePlateAsync(vehicleRequest.LicensePlate,vehicleRequest.Id))
                throw new ConflictException(field:Fields.License_Plate,VehicleMessages.LICENSE_PLATE_EXISTS);

            if (vehicleRequest.Id  == null)
            { 
                await _vehicleRepository.AddVehicle(vehicle);
            }
            else
            {
                await _vehicleRepository.UpdateVehicle(vehicle);
            }
            return vehicle.Id;
        }

        public async Task<List<DropdownResponse>> GetAllVehiclesDropdown()
        {
            return await _vehicleRepository.GetAllVehiclesDropdown();
        }

        public async Task<Guid> AssignVehicleToDriver(AssignVehicleRequest assignVehicleRequest)
        {
            var vehicleAssignment = _mapper.Map<VehicleAssignment>(assignVehicleRequest);
            await _vehicleRepository.AssignVehicleToDriver(vehicleAssignment);
            return vehicleAssignment.Id;
        }

        public async Task<Guid> ReAssignVehicleToDriver(AssignVehicleRequest assignVehicleRequest)
        {
            var vehicleAssignment = _mapper.Map<VehicleAssignment>(assignVehicleRequest);
            await _vehicleRepository.ReAssignVehicleToDriver(vehicleAssignment);
            return vehicleAssignment.Id;
        }

        public async Task UnAssignVehicleToDriver(Guid id)
        {
            await _vehicleRepository.UnAssignVehicleToDriver(id);
        }

        public async Task<byte[]> ExportVehicleToCsvAsync(PagedRequest request)
        {
            VehicleResponse vehicleResponse = await _vehicleRepository.GetAllVehicles(request);

            var exportData = _mapper.Map<IEnumerable<VehicleExportDTO>>(
                vehicleResponse.Vehicles
            );

            return CsvExportHelper.GenerateCsv(exportData);
        }
    }
}
