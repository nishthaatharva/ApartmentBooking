using ApartmentBooking.Application.Contracts.Application;
using ApartmentBooking.Application.Features.Common;
using ApartmentBooking.Application.UnitOfWork;
using ApartmentBooking.Domain.Entities;
using System.Net;

namespace ApartmentBooking.Application.Features.Bookings.Commands
{
    public sealed class CancelLeaseCommand : IRequest<ApiResponse<string>>
    {
        public Guid BookingId { get; set; }
    }

    public class CancelLeaseCommandHandler(ICommandUnitOfWork command, IQueryUnitOfWork query, ICurrentUserService currentUserService) : IRequestHandler<CancelLeaseCommand, ApiResponse<string>>
    {
        private readonly ICommandUnitOfWork _command = command;
        private readonly IQueryUnitOfWork _query = query;
        private readonly ICurrentUserService _currentUserService = currentUserService;

        public async Task<ApiResponse<string>> Handle(CancelLeaseCommand request, CancellationToken cancellationToken)
        {
            var booking = await _query.QueryRepository<Booking>().GetByIdAsync(request.BookingId.ToString(), false);
            _ = booking ?? throw new Exception("No booking exist");

            var apartment = await _query.QueryRepository<Apartment>().GetWithIncludeAsync(false, ap => ap.Id == booking.ApartmentId, x => x.ApartmentAmenitiesAssociations!);
            _ = apartment ?? throw new Exception("No apartment exist");

            var currentUser = _currentUserService.UserId;
            if (currentUser != booking.CreatedBy)
            {
                throw new Exception("You are not allowed to checkout");
            }
            else
            {
                if (apartment.Status == 1)//if available
                {
                    throw new Exception("This apartment is not reserved to checkout");
                }
                else
                {
                    if(booking.IsOnLease == false)
                    {
                        throw new Exception("This apartment is not on lease.");
                    }
                    else
                    {
                        apartment.Status = 1;
                        booking.IsBook = false;
                    }
                }

                _command.CommandRepository<Apartment>().Update(apartment);
                var result = await _command.SaveAsync(cancellationToken);

                var response = new ApiResponse<string>
                {
                    Success = result > 0,
                    StatusCode = result > 0 ? (int)HttpStatusCode.OK : (int)HttpStatusCode.BadRequest,
                    Data = "Lease cancelled successfully",
                    Message = result > 0 ? "Lease cancelled" : "Failed to cancel lease"
                };

                return response;
            }
        }
    }
}
