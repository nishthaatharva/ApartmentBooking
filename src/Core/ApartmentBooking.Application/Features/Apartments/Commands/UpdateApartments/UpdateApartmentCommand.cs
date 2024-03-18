using ApartmentBooking.Application.Features.Apartments.Dtos;
using ApartmentBooking.Application.Features.Common;
using ApartmentBooking.Application.UnitOfWork;
using ApartmentBooking.Domain.Entities;
using System.Net;

namespace ApartmentBooking.Application.Features.Apartments.Commands
{
    public record UpdateApartmentCommandRequest : IRequest<ApiResponse<string>>
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Location { get; set; }
        public int Size { get; set; }
        public int Rooms { get; set; }
        public int Status { get; set; }
        public List<string>? ApartmentAmenitiesAssociation { get; set; }
    }

    public class UpdateApartmentCommandHandler(ICommandUnitOfWork command, IQueryUnitOfWork query) : IRequestHandler<UpdateApartmentCommandRequest, ApiResponse<string>>
    {
        private readonly ICommandUnitOfWork _command = command;
        private readonly IQueryUnitOfWork _query = query;
        public async Task<ApiResponse<string>> Handle(UpdateApartmentCommandRequest request, CancellationToken cancellationToken)
        {
            var apartment = await _query.QueryRepository<Apartment>().GetWithIncludeAsync(false, ap => ap.Id == request.Id, x => x.ApartmentAmenitiesAssociations);
            _ = apartment ?? throw new Exception("Apartment not found");

            apartment.Name = request.Name;
            apartment.Location = request.Location;
            apartment.Size = request.Size;
            apartment.Rooms = request.Rooms;
            apartment.Status = request.Status;

            //update amenities list
            var removeAmenities = new List<ApartmentAmenitiesAssociation>();

            //delete amenities if unselected/removed
            if(apartment.ApartmentAmenitiesAssociations != null)
            {
                foreach(var existingAmenities in apartment.ApartmentAmenitiesAssociations)
                {
                    if(!request.ApartmentAmenitiesAssociation!.Any(a => new Guid(a) == existingAmenities.AmenitiesId))
                    {
                        apartment.ApartmentAmenitiesAssociations.Remove(existingAmenities);
                        removeAmenities.Add(existingAmenities);
                    }
                }
            }

            if(removeAmenities.Count > 0)
            {
                _command.CommandRepository<ApartmentAmenitiesAssociation>().RemoveRange(removeAmenities);
            }

            //add newly selected/added amenities
            if(apartment.ApartmentAmenitiesAssociations is null)
            {
                apartment.ApartmentAmenitiesAssociations = new List<ApartmentAmenitiesAssociation>();
            }

            foreach(var item in request.ApartmentAmenitiesAssociation)
            {
                if(!apartment.ApartmentAmenitiesAssociations.Any(x => x.AmenitiesId == new Guid(item)))
                {
                    apartment.ApartmentAmenitiesAssociations.Add(new ApartmentAmenitiesAssociation()
                    {
                        AmenitiesId = Guid.Parse(item),
                        ApartmentId = apartment.Id
                    });
                }
            }

            _command.CommandRepository<Apartment>().Update(apartment);
            var result = await _command.SaveAsync(cancellationToken);
            var response = new ApiResponse<string>
            {
                Success = result > 0,
                StatusCode = result > 0 ? (int)HttpStatusCode.OK : (int)HttpStatusCode.BadRequest,
                Data = "Apartment updated successfully",
                Message = result > 0 ? "Apartment updated" : "Failed to update apartment"
            };
            return response;
        }
    }
}