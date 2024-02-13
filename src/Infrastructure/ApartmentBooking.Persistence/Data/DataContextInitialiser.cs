using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace ApartmentBooking.Persistence.Data;

public static class DataContextInitialiserExtensions
{
    public static async Task InitialiseAppDatabaseAsync(this IServiceProvider services)
    {
        using var scope = services.CreateScope();

        var initialiser = scope.ServiceProvider.GetRequiredService<AppDbContextInitialiser>();
        await initialiser.InitialiseAsync();
    }

    public class AppDbContextInitialiser(ILogger<DataContext> logger, DataContext context)
    {
        private readonly ILogger<DataContext> _logger = logger;

        private readonly DataContext _context = context;

        public async Task InitialiseAsync()
        {
            var pendingMigrations = await _context.Database.GetPendingMigrationsAsync();
            if (pendingMigrations.Any())
            {

                try
                {
                    await _context.Database.MigrateAsync();

                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "An error occurred while initialising the database.");
                    throw;
                }
            }
            var lastAppliedMigration = (await _context.Database.GetAppliedMigrationsAsync()).Last();

            Console.WriteLine($"You're on schema version: {lastAppliedMigration}");
        }
    }
}
