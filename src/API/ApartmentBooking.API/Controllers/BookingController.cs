using ApartmentBooking.API.Controllers.Base;
using ApartmentBooking.Application.Features.Bookings.Commands;
using ApartmentBooking.Application.Features.Common;
using ApartmentBooking.Identity.Authorization;
using ApartmentBooking.Identity.Authorization.Permissions;
using Microsoft.AspNetCore.Mvc;
using Action = ApartmentBooking.Identity.Authorization.Action;

namespace ApartmentBooking.API.Controllers;

public class BookingController : BaseApiController
{
    [HttpPost("Book-Apartment")]
    [MustHavePermission(Action.Create, Resource.Booking)]
    public async Task<ApiResponse<string>> BookApartments(BookApartmentCommandRequest request)
    {
        return await Mediator.Send(request);
    }

    [HttpPost("Checkout-Apartment")]
    [MustHavePermission(Action.Create, Resource.Booking)]
    public async Task<ApiResponse<string>> CheckoutApartment(CheckoutApartmentCommand request)
    {
        return await Mediator.Send(request);
    }

    [HttpPost("Cancel-Lease")]
    [MustHavePermission(Action.Create, Resource.Booking)]
    public async Task<ApiResponse<string>> CancelLease(CancelLeaseCommand request)
    {
        return await Mediator.Send(request);
    }
}
