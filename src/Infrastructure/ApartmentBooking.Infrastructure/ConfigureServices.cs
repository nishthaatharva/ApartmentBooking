using ApartmentBooking.Application.Interfaces;
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
            services.AddServices();

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

        internal static IServiceCollection AddServices(this IServiceCollection services) =>
        services.AddServices(typeof(ITransientService), ServiceLifetime.Transient);

        internal static IServiceCollection AddServices(this IServiceCollection services, Type interfaceType, ServiceLifetime lifetime)
        {
            var interfaceTypes =
                AppDomain.CurrentDomain.GetAssemblies()
                    .SelectMany(s => s.GetTypes())
                    .Where(t => interfaceType.IsAssignableFrom(t)
                                && t.IsClass && !t.IsAbstract)
                    .Select(t => new
                    {
                        Service = t.GetInterfaces().FirstOrDefault(),
                        Implementation = t
                    })
                    .Where(t => t.Service is not null
                                && interfaceType.IsAssignableFrom(t.Service));

            foreach (var type in interfaceTypes)
            {
                services.AddService(type.Service!, type.Implementation, lifetime);
            }

            return services;
        }

        internal static IServiceCollection AddService(this IServiceCollection services, Type serviceType, Type implementationType, ServiceLifetime lifetime) =>
        lifetime switch
        {
        ServiceLifetime.Transient => services.AddTransient(serviceType, implementationType),
        ServiceLifetime.Scoped => services.AddScoped(serviceType, implementationType),
        ServiceLifetime.Singleton => services.AddSingleton(serviceType, implementationType),
        _ => throw new ArgumentException("Invalid lifeTime", nameof(lifetime))
        };
    }
}
