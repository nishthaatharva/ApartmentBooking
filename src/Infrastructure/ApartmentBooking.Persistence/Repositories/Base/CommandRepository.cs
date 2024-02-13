using ApartmentBooking.Application.Contracts.Infrastructure.Repositories.Base;
using ApartmentBooking.Domain.Common;
using ApartmentBooking.Persistence.Data;

namespace ApartmentBooking.Persistence.Repositories.Base
{
    public class CommandRepository<T>(DataContext context) : ICommandRepository<T> where T : BaseEntity, new()
    {
        private readonly DataContext _context = context;
        public async Task<T> AddAsync(T entity)
        {
            await _context.Set<T>().AddAsync(entity);
            return entity;
        }

        public T Update(T entity)
        {
            _context.Set<T>().Update(entity);
            return entity;
        }

        public void Remove(T entity)
        {
            _context.Set<T>().Remove(entity);
        }

        public void RemoveRange(IEnumerable<T> entities)
        {
            _context.Set<T>().RemoveRange(entities);
        }
    }
}
