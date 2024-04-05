using ApartmentBooking.Application.Features.Common;
using ApartmentBooking.Application.UnitOfWork;
using ApartmentBooking.Domain.Entities;
using AutoMapper;
using System.Net;

namespace ApartmentBooking.Application.Features.Bookings.Commands
{
    public sealed class BookApartmentCommandRequest : IRequest<ApiResponse<string>>
    {
        public DateTime BookFrom { get; set; }
        public DateTime BookTill { get; set; }
        public bool IsOnLease { get; set; } = false;
        public int? LeaseDuration { get; set; } //1. week 2.month 3.year
        public Guid ApartmentId { get; set; }
    }

    public class BookApartmentCommandHandler(ICommandUnitOfWork command, IQueryUnitOfWork query, IMapper mapper) : IRequestHandler<BookApartmentCommandRequest, ApiResponse<string>>
    {
        private readonly ICommandUnitOfWork _command = command;
        private readonly IQueryUnitOfWork _query = query;
        private readonly IMapper _mapper = mapper;

        public async Task<ApiResponse<string>> Handle(BookApartmentCommandRequest request, CancellationToken cancellationToken)
        {
            try
            {
                // Begin transaction - lock
                await _command.BeginTransactionAsync();
                var apartment = await _query.QueryRepository<Apartment>().GetWithIncludeAsync(false, ap => ap.Id == request.ApartmentId, x => x.ApartmentAmenitiesAssociations!);
                _ = apartment ?? throw new Exception("No apartment exist");

                if (apartment.Status == 2)
                {
                    throw new Exception($"{apartment.Name} is already reserved");
                }
                else
                {
                    var booking = _mapper.Map<Booking>(request);

                    //lease management
                    if (request.IsOnLease && request.LeaseDuration.HasValue)
                    {
                        booking!.BookTill = request.LeaseDuration.Value switch
                        {
                            // week
                            1 => request.BookFrom.AddDays(7),
                            // month
                            2 => request.BookFrom.AddMonths(1),
                            // year
                            3 => request.BookFrom.AddYears(1),
                            _ => throw new Exception("Invalid lease duration selected"),
                        };
                    }

                    apartment.Status = 2; //reserve
                    
                    await _command.CommandRepository<Booking>().AddAsync(booking!);
                    _command.CommandRepository<Apartment>().Update(apartment);
                    //commit transaction - release lock
                    await _command.CommitTransactionAsync();
                    var result = await _command.SaveAsync(cancellationToken);

                    var response = new ApiResponse<string>
                    {
                        Success = result > 0,
                        StatusCode = result > 0 ? (int)HttpStatusCode.OK : (int)HttpStatusCode.BadRequest,
                        Data = "Apartment booked successfully",
                        Message = result > 0 ? "Apartment booked" : "Failed to book apartment"
                    };

                    return response;
                }
            }   
            catch(Exception ex) 
            { 
                //release lock and discard changes
                await _command.RollbackTransactionAsync();

                return new ApiResponse<string>
                {
                    Success = false,
                    StatusCode = (int)HttpStatusCode.InternalServerError,
                    Data = "Transaction failed, please try after some time",
                    Message = $"An error occurred: {ex.Message}"
                };
            }           
        }
    }
}
