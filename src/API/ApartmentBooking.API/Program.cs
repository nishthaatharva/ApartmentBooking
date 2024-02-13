using ApartmentBooking.Identity.Data;
using ApartmentBooking.API;
using ApartmentBooking.Persistence.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

var app = builder
    .ConfigureServices()
    .ConfigurePipeline();

await app.Services.InitialiseDatabaseAsync();
await app.Services.InitialiseAppDatabaseAsync();

app.Run();
