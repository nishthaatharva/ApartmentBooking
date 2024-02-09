using ApartmentBooking.Application.Model.Authentication;
using ApartmentBooking.Identity.Authorization.Permissions;
using ApartmentBooking.Identity.Data;
using ApartmentBooking.Identity.Interceptors;
using ApartmentBooking.Identity.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace ApartmentBooking.Identity
{
    public static class ConfigureServices
    {
        public static void AddIdentityService(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<JwtSettings>(configuration.GetSection("JwtSettings"));

            services.AddDbContext<AppIdentityDbContext>((sp, options) =>
            {
                options.AddInterceptors(sp.GetRequiredService<AuditableEntitySaveChangesInterceptor>());

                options.UseSqlServer(configuration.GetConnectionString("SqlConnection"),
                    b => b.MigrationsAssembly(typeof(AppIdentityDbContext).Assembly.FullName));
            });

            services.AddScoped<AuditableEntitySaveChangesInterceptor>();
            services.AddScoped<AppIdentityDbContextInitialiser>();
            services.AddIdentity<ApplicationUser, ApplicationRole>().AddEntityFrameworkStores<AppIdentityDbContext>().AddDefaultTokenProviders();

            services.AddSingleton<IAuthorizationPolicyProvider, PermissionPolicyProvider>()
                .AddScoped<IAuthorizationHandler, PermissionAuthorizaionHandler>();
           

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
                 .AddJwtBearer(o =>
                 {
                     o.RequireHttpsMetadata = false;
                     o.SaveToken = false;
#pragma warning disable CS8604 // Possible null reference argument.
                     o.TokenValidationParameters = new TokenValidationParameters
                     {
                         ValidateIssuerSigningKey = true,
                         ValidateIssuer = true,
                         ValidateAudience = true,
                         ValidateLifetime = true,
                         ClockSkew = TimeSpan.Zero,
                         ValidIssuer = configuration["JwtSettings:Issuer"],
                         ValidAudience = configuration["JwtSettings:Audience"],
                         IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JwtSettings:Key"]))
                     };
#pragma warning restore CS8604 // Possible null reference argument.

                     o.Events = new JwtBearerEvents()
                     {
                         OnChallenge = context =>
                         {
                             context.HandleResponse();
                             if (!context.Response.HasStarted)
                             {
                                 throw new Exception("Authentication Failed.");
                             }

                             return Task.CompletedTask;
                         },
                         OnForbidden = _ => throw new Exception("You are not authorized to access this resource."),
                     };
                 });
        }
    }
}
