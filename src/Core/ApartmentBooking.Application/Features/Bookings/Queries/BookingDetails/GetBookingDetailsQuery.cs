using ApartmentBooking.Application.Contracts.Identity;
using ApartmentBooking.Application.Extensions;
using ApartmentBooking.Application.Features.Bookings.Dtos;
using ApartmentBooking.Application.Features.Common;
using ApartmentBooking.Application.UnitOfWork;
using ApartmentBooking.Domain.Entities;
using ApartmentBooking.Domain.Enums;
using System.Net;

namespace ApartmentBooking.Application.Features.Bookings.Queries;

public record GetBookingDetailsQuery(Guid id) : IRequest<ApiResponse<BookingDetailsDto>>;

public class GetBookingDetailsQueryHandler(IQueryUnitOfWork query, IUsersService usersService) : IRequestHandler<GetBookingDetailsQuery, ApiResponse<BookingDetailsDto>>
{
    private readonly IQueryUnitOfWork _query = query;
    private readonly IUsersService _usersService = usersService;
    public async Task<ApiResponse<BookingDetailsDto>> Handle(GetBookingDetailsQuery request, CancellationToken cancellationToken)
    {
        var booking = await _query.QueryRepository<Booking>().GetByIdAsync(request.id.ToString(), false);

        var apartment = await _query.QueryRepository<Apartment>().GetWithIncludeAsync(false, a => a.Id == booking.ApartmentId, x => x.ApartmentAmenitiesAssociations!);

        var bookingDetails = new BookingDetailsDto
        {
            Id = booking.Id,
            BookFrom = booking.BookFrom,
            BookTill = booking.BookTill,
            IsOnLease = booking.IsOnLease,
            ApartmentName = apartment.Name,
            BookedBy = await _usersService.GetNameOfUser(booking.CreatedBy!)
        };

        if(booking.IsOnLease == true)
        {
            bookingDetails.LeaseDurationTime = CommonFunction.GetEnumDisplayName((LeaseDuration)booking.LeaseDuration!);
        }

        if(apartment.ApartmentAmenitiesAssociations != null)
        {
            bookingDetails.Amenities = apartment.ApartmentAmenitiesAssociations.Select(x => x.AmenitiesId.ToString()).ToList();
        }

        var response = new ApiResponse<BookingDetailsDto>
        {
            Success = bookingDetails != null,
            StatusCode = bookingDetails != null ? (int)HttpStatusCode.OK : (int)HttpStatusCode.BadRequest,
            Data = bookingDetails!,
            Message = bookingDetails != null ? "Booking data found" : "Booking data not found"
        };

        return response;
    }
}