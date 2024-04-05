using ApartmentBooking.Application.Contracts.Infrastructure.Repositories.Apartments;
using ApartmentBooking.Application.Contracts.Responses;
using ApartmentBooking.Application.Extensions;
using ApartmentBooking.Application.Features.Bookings.Dtos;
using ApartmentBooking.Application.Features.Common;
using ApartmentBooking.Domain.Entities;
using ApartmentBooking.Persistence.Data;
using ApartmentBooking.Persistence.Repositories.Base;
using ApartmentBooking.Persistence.Services;
using Ardalis.Specification;
using Microsoft.EntityFrameworkCore;

namespace ApartmentBooking.Persistence.Repositories.Bookings
{
    public class BookingQueryRepository : QueryRepository<Booking>, IBookingQueryRepository
    {
        public BookingQueryRepository(DataContext context) : base(context)
        {
        }

        public async Task<IPagedDataResponse<BookingListDto>> SearchAsync(ISpecification<BookingListDto> spec, int pageNumber, int pageSize, CancellationToken cancellationToken, string userId)
        {
            var bookingList = await (from b in _context.Bookings.AsNoTracking()
                                     join a in _context.Apartments.AsNoTracking() on b.ApartmentId equals a.Id
                                     where b.CreatedBy == userId
                                     select new BookingListDto()
                                     {
                                         Id = b.Id,
                                         BookFrom = b.BookFrom,
                                         BookTill = b.BookTill,
                                         ApartmentName = a.Name,
                                         IsOnLease = b.IsOnLease,
                                         IsBook = b.IsBook,
                                         Status = a.Status,
                                         BookFromDisplay = CommonFunction.ConvertDateToStringForDisplay(b.BookFrom),
                                         BookTillDisplay = CommonFunction.ConvertDateToStringForDisplay(b.BookTill),
                                     }).ToListAsync<BookingListDto>(cancellationToken: cancellationToken);

            var booking = bookingList.ApplySpecification(spec);
            var count = bookingList.ApplySpecificationToListCount(spec);

            return new PagedApiResponse<BookingListDto>(count, pageNumber, pageSize) { Data = booking };
        }
    }
}
