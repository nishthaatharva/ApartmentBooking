﻿using ApartmentBooking.Application.Contracts.Application;
using ApartmentBooking.Domain.Common;
using ApartmentBooking.Domain.Common.Contracts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace ApartmentBooking.Persistence.Interceptors
{
    public class AuditableEntitySaveChangesInterceptor(ICurrentUserService currentUserService) : SaveChangesInterceptor
    {
        private readonly ICurrentUserService _currentUserService = currentUserService;

        public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
        {
            UpdateEntities(eventData.Context);

            return base.SavingChanges(eventData, result);
        }
        public override ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken = default)
        {
            UpdateEntities(eventData.Context);

            return base.SavingChangesAsync(eventData, result, cancellationToken);
        }

        public void UpdateEntities(DbContext? context)
        {
            if (context == null) return;

            DateTime now = DateTime.UtcNow;

            foreach (var entry in context.ChangeTracker.Entries<BaseAuditableEntity>())
            {
                if (entry.State == EntityState.Added)
                {
                    entry.Entity.CreatedBy = _currentUserService.UserId;
                    entry.Entity.CreatedOn = now;
                }

                if (entry.State == EntityState.Added || entry.State == EntityState.Modified)
                {
                    entry.Entity.ModifiedBy = _currentUserService.UserId;
                    entry.Entity.ModifiedOn = now;
                }

                if (entry.State == EntityState.Deleted)
                {
                    if (entry.Entity is ISoftDelete softDelete)
                    {
                        softDelete.IsDeleted = true;
                        softDelete.DeletedBy = _currentUserService.UserId;
                        softDelete.DeletedOn = now;
                        entry.State = EntityState.Modified;
                    }
                }
            }
        }
    }
}
