using ApartmentBooking.Domain.Common;

namespace ApartmentBooking.Application.Contracts.Infrastructure.Repositories.Base
{
    public interface ICommandRepository<T> where T : BaseEntity, new()
    {
        Task<T> AddAsync(T entity);
        T Update(T entity);
        void Remove(T entity);
    }
}
