using ApartmentBooking.Application.Contracts.Infrastructure.Repositories.Base;
using ApartmentBooking.Application.Contracts.Responses;
using ApartmentBooking.Application.Features.Bookings.Dtos;
using ApartmentBooking.Domain.Entities;
using Ardalis.Specification;

namespace ApartmentBooking.Application.Contracts.Infrastructure.Repositories.Apartments
{
    public interface IBookingQueryRepository : IQueryRepository<Booking>
    {
        Task<IPagedDataResponse<BookingListDto>> SearchAsync(ISpecification<BookingListDto> spec,
                                                            int pageNumber,
                                                            int pageSize,
                                                            CancellationToken cancellationToken,
                                                            string userId);
    }
}
