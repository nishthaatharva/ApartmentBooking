using ApartmentBooking.Domain.Common.Contracts;
using ApartmentBooking.Identity.Interceptors;
using ApartmentBooking.Identity.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ApartmentBooking.Identity.Data
{
    public class AppIdentityDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, string, IdentityUserClaim<string>, IdentityUserRole<string>, ApplicationUserLogin, ApplicationRoleClaim, IdentityUserToken<string>>
    {
        private readonly AuditableEntitySaveChangesInterceptor _auditableEntitySaveChangesInterceptor;
        public AppIdentityDbContext(DbContextOptions<AppIdentityDbContext> options,
            AuditableEntitySaveChangesInterceptor auditableEntitySaveChangesInterceptor) : base(options) 
        {
            _auditableEntitySaveChangesInterceptor = auditableEntitySaveChangesInterceptor;
        }
       
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.AddInterceptors(_auditableEntitySaveChangesInterceptor);
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            // QueryFilters need to be applied before base.OnModelCreating
            builder.AppendGlobalQueryFilter<ISoftDelete>(s => s.IsDeleted == false);

            base.OnModelCreating(builder);
            builder.ApplyConfigurationsFromAssembly(GetType().Assembly);
        }
    }
}
