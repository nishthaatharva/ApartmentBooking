using ApartmentBooking.Application.Interfaces;

namespace ApartmentBooking.Application.Contracts.Caching
{
    public interface ICacheKeyService : IScopedService
    {
        public string GetCacheKey(string name, object id);
    }
}
