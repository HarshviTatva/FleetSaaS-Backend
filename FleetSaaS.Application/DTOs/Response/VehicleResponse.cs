namespace FleetSaaS.Application.DTOs.Response
{
    public class VehicleResponse : PagedResponse
    {
        public List<VehicleDTO> Vehicles { get; set; }
        public Guid CompanyId { get; set; }
    }

    public class VehicleDTO
    {
        public Guid? Id { get; set; }
        public string Make { get; set; }
        public string Model { get; set; }
        public int Year { get; set; }
        public string Vin { get; set; }
        public DateOnly? InsuranceExpiryDate { get; set; }
        public bool? IsActive { get; set; }
        public string LicensePlate { get; set; }
    }

    public class VehicleExportDTO
    {
        public string Make { get; set; }
        public string Model { get; set; }
        public int Year { get; set; }
        public string Vin { get; set; }
        public DateOnly? InsuranceExpiryDate { get; set; }
        public string LicensePlate { get; set; }
    }
}
