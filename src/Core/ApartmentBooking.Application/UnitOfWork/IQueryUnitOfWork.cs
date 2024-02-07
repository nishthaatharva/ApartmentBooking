using ApartmentBooking.Application.Contracts.Infrastructure.Repositories.Apartments;
using ApartmentBooking.Application.Contracts.Infrastructure.Repositories.Base;
using ApartmentBooking.Domain.Common;

namespace ApartmentBooking.Application.UnitOfWork
{
    public interface IQueryUnitOfWork : IDisposable
    {
        IQueryRepository<TEntity> QueryRepository<TEntity>() where TEntity : BaseEntity, new();

        IApartmentQueryRepository ApartmentQuery { get; }
    }
}
