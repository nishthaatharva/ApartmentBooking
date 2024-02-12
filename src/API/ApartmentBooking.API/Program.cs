
using ApartmentBooking.Identity.Data;
using ApartmentBooking.API;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.




var app = builder
    .ConfigureServices()
    .ConfigurePipeline();

// Configure the HTTP request pipeline.


await app.Services.InitialiseDatabaseAsync();

app.Run();


