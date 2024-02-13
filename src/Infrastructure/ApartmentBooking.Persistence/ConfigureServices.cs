using ApartmentBooking.Application.UnitOfWork;
using ApartmentBooking.Persistence.Data;
using ApartmentBooking.Persistence.Interceptors;
using ApartmentBooking.Persistence.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using static ApartmentBooking.Persistence.Data.DataContextInitialiserExtensions;

namespace ApartmentBooking.Persistence;

public static class ConfigureServices
{
    public static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<AuditableEntitySaveChangesInterceptor>();
        services.AddDbContext<DataContext>((sp, options) =>
        {
            options.AddInterceptors(
               sp.GetRequiredService<AuditableEntitySaveChangesInterceptor>()
           );

            options.UseSqlServer(configuration.GetConnectionString("SqlConnection"));
        });

        services.AddScoped<AppDbContextInitialiser>();

        services.AddScoped<ICommandUnitOfWork, CommandUnitOfWork>();
        services.AddScoped<IQueryUnitOfWork, QueryUnitOfWork>();

        return services;
    }
}
