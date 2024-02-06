﻿using ApartmentBooking.Application.Features.Apartments.Dtos;
using ApartmentBooking.Application.Features.Common;
using ApartmentBooking.Application.UnitOfWork;
using ApartmentBooking.Domain.Entities;
using System.Net;

namespace ApartmentBooking.Application.Features.Apartments.Commands
{
    public record UpdateApartmentCommandRequest(ApartmentDto apartment) : IRequest<ApiResponse<string>>;

    public class UpdateApartmentCommandHandler(ICommandUnitOfWork command, IQueryUnitOfWork query) : IRequestHandler<UpdateApartmentCommandRequest, ApiResponse<string>>
    {
        private readonly ICommandUnitOfWork _command = command;
        private readonly IQueryUnitOfWork _query = query;
        public async Task<ApiResponse<string>> Handle(UpdateApartmentCommandRequest request, CancellationToken cancellationToken)
        {
            var apartment = await _query.QueryRepository<Apartment>().GetWithIncludeAsync(false, ap => ap.Id == request.apartment.Id, x => x.ApartmentAmenitiesAssociations);
            _ = apartment ?? throw new Exception("Apartment not found");

            apartment.Name = request.apartment.Name;
            apartment.Location = request.apartment.Location;
            apartment.Size = request.apartment.Size;
            apartment.Rooms = request.apartment.Rooms;
            apartment.Status = request.apartment.Status;

            //update amenities list
            var removeAmenities = new List<ApartmentAmenitiesAssociation>();

            //delete amenities if unselected/removed
            if(apartment.ApartmentAmenitiesAssociations != null)
            {
                foreach(var existingAmenities in apartment.ApartmentAmenitiesAssociations)
                {
                    if(!request.apartment.ApartmentAmenitiesAssociations.Any(a => new Guid(a) == existingAmenities.Id))
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

            foreach(var item in request.apartment.ApartmentAmenitiesAssociations)
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