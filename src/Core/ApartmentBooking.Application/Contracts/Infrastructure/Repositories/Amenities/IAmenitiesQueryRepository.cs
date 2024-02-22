using ApartmentBooking.Application.Contracts.Infrastructure.Repositories.Base;
using ApartmentBooking.Domain.Entities;

namespace ApartmentBooking.Application.Contracts.Infrastructure.Repositories
{
    public interface IAmenitiesQueryRepository : IQueryRepository<Amenities>
    {
        Task<List<string>> GetAmenitiesName(List<Guid> amenitiesIds, CancellationToken cancellationToken);
    }
}
