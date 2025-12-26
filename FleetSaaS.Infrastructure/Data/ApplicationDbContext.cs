using FleetSaaS.Domain.Entities;
using FleetSaaS.Domain.Interface;
using FleetSaaS.Infrastructure.Common;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace FleetSaaS.Infrastructure.Data
{
    public class ApplicationDbContext :DbContext
    {
        private readonly ITenantProvider _tenantProvider;

        public ApplicationDbContext(
            DbContextOptions<ApplicationDbContext> options,
            ITenantProvider tenantProvider)
            : base(options)
        {
            _tenantProvider = tenantProvider;
        }

        // DbSets
        public DbSet<Company> Companies { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        //public DbSet<UserRole> UserRoles { get; set; }
        public DbSet<Driver> Drivers { get; set; }
        public DbSet<Vehicle> Vehicles { get; set; }
        public DbSet<VehicleAssignment> VehicleAssignments { get; set; }
        public DbSet<Trip> Trips { get; set; }
        public DbSet<TripOdometerLog> TripOdometerLogs { get; set; }
        public DbSet<AuditLogs> AuditLogs { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            ConfigureRelationships(modelBuilder);
            ApplyTenantFilter(modelBuilder);
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

        private LambdaExpression BuildTenantFilterExpression(Type entityType)
        {
            var parameter = Expression.Parameter(entityType, "e");
            var property = Expression.Property(parameter, nameof(ITenantEntity.CompanyId));
            var tenantId = Expression.Constant(_tenantProvider.CompanyId);

            var body = Expression.Equal(property, tenantId);
            return Expression.Lambda(body, parameter);
        }


        private void ApplyTenantFilter(ModelBuilder modelBuilder)
        {
            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                if (typeof(ITenantEntity).IsAssignableFrom(entityType.ClrType))
                {
                    modelBuilder.Entity(entityType.ClrType)
                        .HasQueryFilter(BuildTenantFilterExpression(entityType.ClrType));
                }
            }
        }
    }
}
