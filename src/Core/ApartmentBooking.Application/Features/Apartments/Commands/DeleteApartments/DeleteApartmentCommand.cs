using ApartmentBooking.Application.Features.Common;
using ApartmentBooking.Application.UnitOfWork;
using ApartmentBooking.Domain.Entities;
using System.Net;

namespace ApartmentBooking.Application.Features.Apartments.Commands
{
    public record DeleteApartmentCommandRequest(Guid id) : IRequest<ApiResponse<string>>;

    public class DeleteApartmentCommandHandler(IQueryUnitOfWork query, 
        ICommandUnitOfWork command) : IRequestHandler<DeleteApartmentCommandRequest, ApiResponse<string>>
    {
        private readonly ICommandUnitOfWork _command = command;
        private readonly IQueryUnitOfWork _query = query;

        public async Task<ApiResponse<string>> Handle(DeleteApartmentCommandRequest request, CancellationToken cancellationToken)
        {
            var apartment = await _query.QueryRepository<Apartment>().GetWithIncludeAsync(false, x => x.Id == request.id, x => x.ApartmentAmenitiesAssociations!);
            _ = apartment ?? throw new Exception("Apartment not found");

            //check apartment reservation
            if(apartment.Status == 2)
            {
                throw new Exception("Cannot delete apartment as it is reserved");
            }

            _command.CommandRepository<Apartment>().Remove(apartment);

            if(apartment.ApartmentAmenitiesAssociations!.Count > 0)
            {
                _command.CommandRepository<ApartmentAmenitiesAssociation>().RemoveRange(apartment.ApartmentAmenitiesAssociations);
            }

            var result = await _command.SaveAsync(cancellationToken);

            var response = new ApiResponse<string>
            {
                Success = result > 0,
                StatusCode = result > 0 ? (int)HttpStatusCode.OK : (int)HttpStatusCode.BadRequest,
                Data = "Apartment deleted successfully",
                Message = result > 0 ? "Apartment deleted" : "Failed to delete apartment"
            };
            return response;
        }
    }
}
