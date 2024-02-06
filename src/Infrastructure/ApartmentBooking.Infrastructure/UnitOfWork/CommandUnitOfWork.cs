using ApartmentBooking.Application.Contracts.Infrastructure.Repositories.Base;
using ApartmentBooking.Application.UnitOfWork;
using ApartmentBooking.Domain.Common;
using ApartmentBooking.Infrastructure.Data;
using ApartmentBooking.Infrastructure.Repositories.Base;
using System.Collections;

namespace ApartmentBooking.Infrastructure.UnitOfWork
{
    public class CommandUnitOfWork : ICommandUnitOfWork
    {
        private readonly DataContext _context;
        private Hashtable _repositories;
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        public CommandUnitOfWork(DataContext context)
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        {
            _context = context;
        }  

        public async Task<int> SaveAsync(CancellationToken cancellationToken)
        {
            var result = await _context.SaveChangesAsync(cancellationToken);
            return result;
        }

        public ICommandRepository<TEntity> CommandRepository<TEntity>() where TEntity : BaseEntity, new()
        {
            if(_repositories == null) _repositories = new Hashtable();

            var type = typeof(TEntity).Name;

            if(!_repositories.ContainsKey(type))
            {
                var repositoryType = typeof(CommandRepository<>);
                var repositoryInstance = Activator.CreateInstance(repositoryType.MakeGenericType(typeof(TEntity)), _context);

                _repositories.Add(type, repositoryInstance);
            }
            // Ensure _repositories[type] is not null before returning
#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
            return (ICommandRepository<TEntity>)_repositories[type] ?? new CommandRepository<TEntity>(_context);
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}