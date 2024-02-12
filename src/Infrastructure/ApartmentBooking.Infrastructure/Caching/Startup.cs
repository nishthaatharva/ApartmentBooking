using ApartmentBooking.Application.Contracts.Caching;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ApartmentBooking.Infrastructure.Caching
{
    internal static class Startup
    {
        internal static IServiceCollection AddCaching(this IServiceCollection services)
        {
            services.AddTransient<ICacheService, LocalCacheService>();
           
            services.AddMemoryCache();
            return services;
        }
    }
}
