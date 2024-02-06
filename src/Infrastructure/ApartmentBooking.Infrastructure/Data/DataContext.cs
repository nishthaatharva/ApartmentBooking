using ApartmentBooking.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace ApartmentBooking.Infrastructure.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }

        public DbSet<Apartment> Apartments => Set<Apartment>();
        public DbSet<Amenities> Amenities => Set<Amenities>();
        public DbSet<ApartmentAmenitiesAssociation> ApartmentAmenitiesAssociations => Set<ApartmentAmenitiesAssociation>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Seed Amenities data
            modelBuilder.Entity<Amenities>().HasData(
                new Amenities {Id= new Guid("D0F230C1-A0BD-4085-7239-08DC02176302"),  Name = "Gym" },
                new Amenities {Id = new Guid("7984DCE3-39A8-4DA3-BFAC-08DC103B5C3F"),  Name = "Pool" },
                new Amenities {Id = new Guid("70EDB255-7BF7-4597-D02C-08DC10D6610E"),  Name = "Garden" }
            );
        }
    }
}
