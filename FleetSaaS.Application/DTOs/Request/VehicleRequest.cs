namespace FleetSaaS.Application.DTOs.Request
{
    public class VehicleRequest
    {
        public Guid? Id {  get; set; }
        public string Make { get; set; }
        public string Model { get; set; }
        public int Year { get; set; }
        public string Vin { get; set; }
        public DateOnly InsuranceExpiryDate { get; set; }
        public bool? IsActive { get; set; }
        public string LicensePlate { get; set; }
        public Guid? CompanyId { get; set; }
    }
}
