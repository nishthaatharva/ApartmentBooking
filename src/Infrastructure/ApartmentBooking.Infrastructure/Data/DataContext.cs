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
    }
}
