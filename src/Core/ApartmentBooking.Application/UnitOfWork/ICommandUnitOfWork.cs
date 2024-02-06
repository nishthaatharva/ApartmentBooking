using ApartmentBooking.Application.Contracts.Infrastructure.Repositories.Base;
using ApartmentBooking.Domain.Common;

namespace ApartmentBooking.Application.UnitOfWork
{
    public interface ICommandUnitOfWork : IDisposable
    {
        Task<int> SaveAsync(CancellationToken cancellationToken);
        ICommandRepository<TEntity> CommandRepository<TEntity>() where TEntity : BaseEntity, new();
    }
}