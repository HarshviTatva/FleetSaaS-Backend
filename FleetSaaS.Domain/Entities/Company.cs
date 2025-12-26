namespace FleetSaaS.Domain.Entities
{
    public class Company : BaseEntity
    {
        public string CompanyName { get; set; } = default!;
        public string Email { get; set; } = default!;
        public string PhoneNumber { get; set; }
        public bool IsActive { get; set; } = true;
        public ICollection<User> Users { get; set; } = new List<User>();
        public ICollection<Vehicle> Vehicles { get; set; } = new List<Vehicle>();
        public ICollection<Driver> Drivers { get; set; } = new List<Driver>();
        public ICollection<Trip> Trips { get; set; } = new List<Trip>();
    }
}
