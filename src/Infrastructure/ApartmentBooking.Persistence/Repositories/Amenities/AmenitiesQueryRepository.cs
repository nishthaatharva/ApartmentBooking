using ApartmentBooking.Application.Contracts.Infrastructure.Repositories;
using ApartmentBooking.Domain.Entities;
using ApartmentBooking.Persistence.Data;
using ApartmentBooking.Persistence.Repositories.Base;
using Microsoft.EntityFrameworkCore;

namespace ApartmentBooking.Persistence.Repositories.Amenity
{
    public class AmenitiesQueryRepository : QueryRepository<Amenities>, IAmenitiesQueryRepository
    {
        public AmenitiesQueryRepository(DataContext context) : base(context) { }
        public async Task<List<string>> GetAmenitiesName(List<Guid> amenitiesIds, CancellationToken cancellationToken = default)
        {
            var amenitiesNames = await _context.Amenities
               .Where(a => amenitiesIds.Contains(a.Id))
               .Select(a => a.Name)
               .ToListAsync(cancellationToken);

            return amenitiesNames;
        }
    }
}
