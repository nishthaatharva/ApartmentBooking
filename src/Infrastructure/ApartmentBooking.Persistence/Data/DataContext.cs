using ApartmentBooking.Domain.Common.Contracts;
using ApartmentBooking.Domain.Entities;
using ApartmentBooking.Persistence.Interceptors;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace ApartmentBooking.Persistence.Data
{
    public class DataContext : DbContext
    {
        private readonly AuditableEntitySaveChangesInterceptor _auditableEntitySaveChangesInterceptor;
        public DataContext(DbContextOptions<DataContext> options,
            AuditableEntitySaveChangesInterceptor auditableEntitySaveChangesInterceptor) : base(options)
        {
            _auditableEntitySaveChangesInterceptor = auditableEntitySaveChangesInterceptor;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.AddInterceptors(_auditableEntitySaveChangesInterceptor);
        }

        public DbSet<Apartment> Apartments => Set<Apartment>();
        public DbSet<Amenities> Amenities => Set<Amenities>();
        public DbSet<ApartmentAmenitiesAssociation> ApartmentAmenitiesAssociations => Set<ApartmentAmenitiesAssociation>();
        public DbSet<Booking> Bookings => Set<Booking>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.AppendGlobalQueryFilter<ISoftDelete>(s => s.IsDeleted == false);

            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            base.OnModelCreating(modelBuilder);

            // Seed Amenities data
            modelBuilder.Entity<Amenities>().HasData(
                new Amenities { Id = new Guid("D0F230C1-A0BD-4085-7239-08DC02176302"), Name = "Gym" },
                new Amenities { Id = new Guid("7984DCE3-39A8-4DA3-BFAC-08DC103B5C3F"), Name = "Pool" },
                new Amenities { Id = new Guid("70EDB255-7BF7-4597-D02C-08DC10D6610E"), Name = "Garden" }
            );
        }
    }
}
