using ApartmentBooking.API;
using ApartmentBooking.Identity.Data;
using ApartmentBooking.Persistence.Data;
using Serilog;
using Serilog.Events;
using Serilog.Formatting.Compact;

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateBootstrapLogger();

Log.Information("Apartment booking API starting");

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((context, loggerConfiguration) =>
{
    loggerConfiguration
        .ReadFrom.Configuration(context.Configuration)
        .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
        .MinimumLevel.Override("Microsoft.Hosting.Lifetime", LogEventLevel.Information)
        .Enrich.FromLogContext()
        .WriteTo.Console()
        .WriteTo.File(new CompactJsonFormatter(), GetLogFilePath(), rollingInterval: RollingInterval.Day, retainedFileCountLimit: 7)
        .WriteTo.MSSqlServer(context.Configuration.GetConnectionString("SqlConnection"), "Logs", autoCreateSqlTable: true); 
});
static string GetLogFilePath()
{
    // Get the calling method name
    var callingMethod = GetCallingMethodName();
    // Construct the log file path based on the method name
    return $"log/{callingMethod}-log-.txt";
}

static string GetCallingMethodName()
{
    // Get the calling method name by inspecting the call stack
    var stackTrace = new System.Diagnostics.StackTrace();
    var callingMethod = stackTrace.GetFrame(2)?.GetMethod();
    return callingMethod != null ? $"{callingMethod.DeclaringType?.Name}-{callingMethod.Name}" : "Unknown";
}


var app = builder
    .ConfigureServices()
    .ConfigurePipeline();

app.UseSerilogRequestLogging();

await app.Services.InitialiseDatabaseAsync();
await app.Services.InitialiseAppDatabaseAsync();

app.Run();