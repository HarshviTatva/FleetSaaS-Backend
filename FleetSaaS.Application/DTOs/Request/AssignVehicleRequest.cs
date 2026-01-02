namespace FleetSaaS.Application.DTOs.Request
{
    public class AssignVehicleRequest
    {
        public Guid VehicleId { get; set; }
        public Guid DriverId { get; set; }
        public Guid? CompanyId { get; set; }
        public Guid? Id { get; set; }
    }

    public class AssignTripDriverRequest
    {
        public Guid VehicleAssignmentId { get; set; }
        public Guid Id { get; set; }
    }
}
