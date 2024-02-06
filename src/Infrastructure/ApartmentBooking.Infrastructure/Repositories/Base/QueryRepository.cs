using ApartmentBooking.Application.Contracts.Infrastructure.Repositories.Base;
using ApartmentBooking.Domain.Common;
using ApartmentBooking.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace ApartmentBooking.Infrastructure.Repositories.Base
{
    public class QueryRepository<T>(DataContext context) : IQueryRepository<T> where T : BaseEntity, new()
    {
        private readonly DataContext _context = context;

        public async Task<bool> AnyAsync(Expression<Func<T, bool>> predicate)
        {
            var result = await _context.Set<T>().Where(predicate).AnyAsync();
            return result;
        }

        public async Task<IQueryable<T>> GetAllAsync(bool isChangeTracking = false)
        {
            IQueryable<T> query = _context.Set<T>();
            if (isChangeTracking)
            {
                return query = query.AsNoTracking().AsQueryable();
            }
            return await Task.Run(() => query.AsQueryable());


        }

#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
        public async Task<IEnumerable<T>> GetAllWithIncludeAsync(bool isChangeTracking = false, Expression<Func<T, bool>> predicate = null, params Expression<Func<T, object>>[] includes)
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.
        {
            IQueryable<T> query = _context.Set<T>();

            if (isChangeTracking)
            {
                query = predicate is null ? query.AsNoTracking()
                                          : query.Where(predicate).AsNoTracking();
                if (includes != null)
                {
                    foreach (var item in includes)
                    {
                        query = query.Include(item);
                    }
                }
            }
            else
            {
                query = predicate is null ? query
                                          : query.Where(predicate);
                if (includes != null)
                {
                    foreach (var item in includes)
                    {
                        query = query.Include(item);
                    }
                }
            }

            return await query.ToListAsync();

        }

        public async Task<T> GetAsync(Expression<Func<T, bool>> predicate, bool isChangeTracking = false)
        {
            IQueryable<T> query = _context.Set<T>();
            if (isChangeTracking)
            {
                query = query.Where(predicate).AsNoTracking();
            }
            else
            {
                query = query.Where(predicate);
            }
#pragma warning disable CS8603 // Possible null reference return.
            return await query.FirstOrDefaultAsync();
#pragma warning restore CS8603 // Possible null reference return.
        }

        public async Task<T> GetByIdAsync(string id, bool isChangeTracking = false)
        {
            IQueryable<T> query = _context.Set<T>();
            if (isChangeTracking)
            {
                query = query.Where(e => e.Id == Guid.Parse(id)).AsNoTracking();
            }
            else
            {
                query = query.Where(e => e.Id == Guid.Parse(id));
            }

#pragma warning disable CS8603 // Possible null reference return.
            return await query.SingleOrDefaultAsync();
#pragma warning restore CS8603 // Possible null reference return.
        }

        public async Task<T> GetWithIncludeAsync(bool isChangeTracking, Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includes)
        {
            IQueryable<T> query = _context.Set<T>();

            if (isChangeTracking)
            {
                query = query.Where(predicate);
                if (includes is not null)
                {
                    foreach (var item in includes)
                    {
                        query = query.Include(item);
                    }
                }
            }
            else
            {
                query = query.Where(predicate).AsNoTracking();
                if (includes is not null)
                {
                    foreach (var item in includes)
                    {
                        query = query.Include(item);
                    }
                }
            }

#pragma warning disable CS8603 // Possible null reference return.
            return await query.SingleOrDefaultAsync();
#pragma warning restore CS8603 // Possible null reference return.

        }
    }
}
