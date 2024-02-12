using ApartmentBooking.Application.Features.Bookings.Commands;
using ApartmentBooking.Application.Features.Common;
using ApartmentBooking.Identity.Authorization;
using ApartmentBooking.Identity.Authorization.Permissions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Action = ApartmentBooking.Identity.Authorization.Action;

namespace ApartmentBooking.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookingController(IMediator mediator) : ControllerBase
    {
        private readonly IMediator _mediator = mediator;

        [HttpPost("Book-Apartment")]
        [MustHavePermission(Action.Create, Resource.Booking)]
        public async Task<ApiResponse<string>> BookApartments(BookApartmentCommandRequest request)
        {
            return await _mediator.Send(request);
        }
    }
}
