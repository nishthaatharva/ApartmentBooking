﻿using ApartmentBooking.Application.Contracts.Infrastructure.Repositories;
using ApartmentBooking.Application.Contracts.Infrastructure.Repositories.Apartments;
using ApartmentBooking.Application.Contracts.Infrastructure.Repositories.Base;
using ApartmentBooking.Application.UnitOfWork;
using ApartmentBooking.Domain.Common;
using ApartmentBooking.Persistence.Data;
using ApartmentBooking.Persistence.Repositories.Amenity;
using ApartmentBooking.Persistence.Repositories.Apartments;
using ApartmentBooking.Persistence.Repositories.Base;
using ApartmentBooking.Persistence.Repositories.Bookings;
using System.Collections;

namespace ApartmentBooking.Persistence.UnitOfWork
{
    public class QueryUnitOfWork : IQueryUnitOfWork
    {
        private readonly DataContext _context;
        private Hashtable _repositories;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        public QueryUnitOfWork(DataContext context)
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        {
            _context = context;
        }

        public IQueryRepository<TEntity> QueryRepository<TEntity>() where TEntity : BaseEntity, new()
        {
            if (_repositories == null) _repositories = new Hashtable();

            var type = typeof(TEntity).Name;

            if (!_repositories.ContainsKey(type))
            {
                var repositoryType = typeof(QueryRepository<>);
                var repositoryInstance = Activator.CreateInstance(repositoryType.MakeGenericType(typeof(TEntity)), _context);

                _repositories.Add(type, repositoryInstance);
            }
            // Ensure _repositories[type] is not null before returning
#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
            return (IQueryRepository<TEntity>)_repositories[type] ?? new QueryRepository<TEntity>(_context);
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.
        }

        public ApartmentQueryRepository _apartmentRepository;
        public IApartmentQueryRepository ApartmentQuery => _apartmentRepository ?? new ApartmentQueryRepository(_context);

        public AmenitiesQueryRepository _amenitiesRepository;
        public IAmenitiesQueryRepository AmenitiesQuery => _amenitiesRepository ?? new AmenitiesQueryRepository(_context);

        public BookingQueryRepository _bookingRepository;
        public IBookingQueryRepository BookingQuery => _bookingRepository ?? new BookingQueryRepository(_context);

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
