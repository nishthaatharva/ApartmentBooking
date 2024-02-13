using ApartmentBooking.Application.Contracts.Application;
using ApartmentBooking.Application.Features.Common;
using ApartmentBooking.Application.UnitOfWork;
using ApartmentBooking.Domain.Entities;
using System.Net;

namespace ApartmentBooking.Application.Features.Bookings.Commands
{
    public sealed class CheckoutApartmentCommand : IRequest<ApiResponse<string>>
    {
        public Guid BookingId { get; set; }
    }

    public class CheckoutApartmentCommandHandler(ICommandUnitOfWork command, IQueryUnitOfWork query, ICurrentUserService currentUserService) : IRequestHandler<CheckoutApartmentCommand, ApiResponse<string>>
    {
        private readonly ICommandUnitOfWork _command = command;
        private readonly IQueryUnitOfWork _query = query;
        private readonly ICurrentUserService _currentUserService = currentUserService;

        public async Task<ApiResponse<string>> Handle(CheckoutApartmentCommand request, CancellationToken cancellationToken)
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
                else //if reserve
                {
                    if (booking.IsOnLease == false) //not on lease
                    {
                        apartment.Status = 1;
                    }
                    else //on lease
                    {
                        if (DateTime.UtcNow < booking.BookTill)
                        {
                            throw new Exception("Please comple your lease duration or cancel lease");
                        }
                        else
                        {
                            apartment.Status = 1;
                        }
                    }

                    _command.CommandRepository<Apartment>().Update(apartment);
                    var result = await _command.SaveAsync(cancellationToken);

                    var response = new ApiResponse<string>
                    {
                        Success = result > 0,
                        StatusCode = result > 0 ? (int)HttpStatusCode.OK : (int)HttpStatusCode.BadRequest,
                        Data = "Apartment checkout successfully",
                        Message = result > 0 ? "Apartment checkout" : "Failed to checkout apartment"
                    };

                    return response;
                }
            }          
        }
    }
}
