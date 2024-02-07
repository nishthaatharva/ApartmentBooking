using ApartmentBooking.Application.UnitOfWork;
using ApartmentBooking.Infrastructure.Data;
using ApartmentBooking.Infrastructure.Interceptors;
using ApartmentBooking.Infrastructure.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ApartmentBooking.Infrastructure
{
    public static class ConfigureServices
    {
        public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<AuditableEntitySaveChangesInterceptor>();
            services.AddDbContext<DataContext>((sp, options) =>
            {
                options.AddInterceptors(
                   sp.GetRequiredService<AuditableEntitySaveChangesInterceptor>()
               );

                options.UseSqlServer(configuration.GetConnectionString("SqlConnection"));
            });

            services.AddScoped<ICommandUnitOfWork, CommandUnitOfWork>();
            services.AddScoped<IQueryUnitOfWork, QueryUnitOfWork>();
        }
    }
}
