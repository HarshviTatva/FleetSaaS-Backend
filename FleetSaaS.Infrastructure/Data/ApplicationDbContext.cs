using FleetSaaS.Domain.Entities;
using FleetSaaS.Infrastructure.Common;
using Microsoft.EntityFrameworkCore;

namespace FleetSaaS.Infrastructure.Data
{
    public class ApplicationDbContext :DbContext
    {
        private readonly ITenantProvider _tenantProvider;
        public Guid CurrentCompanyId { get; set; }
        public ApplicationDbContext(
            DbContextOptions<ApplicationDbContext> options,
            ITenantProvider tenantProvider)
            : base(options)
        {
            _tenantProvider = tenantProvider;
            CurrentCompanyId = _tenantProvider.CompanyId;
        }

        // DbSets
        public DbSet<Company> Companies { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Driver> Drivers { get; set; }
        public DbSet<Vehicle> Vehicles { get; set; }
        public DbSet<VehicleAssignment> VehicleAssignments { get; set; }
        public DbSet<Trip> Trips { get; set; }
        public DbSet<AuditLogs> AuditLogs { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            ConfigureRelationships(modelBuilder);

            ConfigureQueryFilters(modelBuilder);
        }
      
        private static void ConfigureRelationships(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .HasOne(u => u.Company)
                .WithMany(c => c.Users)
                .HasForeignKey(u => u.CompanyId);

            modelBuilder.Entity<Driver>()
                .HasOne(d => d.User)
                .WithMany()
                .HasForeignKey(d => d.UserId);

            modelBuilder.Entity<VehicleAssignment>()
                .HasOne(va => va.Vehicle)
                .WithMany(v => v.Assignments)
                .HasForeignKey(va => va.VehicleId);

            modelBuilder.Entity<VehicleAssignment>()
                .HasOne(va => va.Driver)
                .WithMany(d => d.VehicleAssignments)
                .HasForeignKey(va => va.DriverId);
            
        }

        private void ConfigureQueryFilters(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .HasQueryFilter(u => u.CompanyId == CurrentCompanyId);

            modelBuilder.Entity<Trip>()
               .HasQueryFilter(u => u.CompanyId == CurrentCompanyId);

            modelBuilder.Entity<Driver>()
                .HasQueryFilter(d => d.User.CompanyId == CurrentCompanyId);

            modelBuilder.Entity<Vehicle>()
                .HasQueryFilter(v => v.CompanyId == CurrentCompanyId);

            modelBuilder.Entity<VehicleAssignment>()
                .HasQueryFilter(va => va.Vehicle.CompanyId == CurrentCompanyId);
        }

    }
}
