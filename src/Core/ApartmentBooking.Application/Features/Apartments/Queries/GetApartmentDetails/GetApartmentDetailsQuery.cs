using ApartmentBooking.Application.Features.Apartments.Dtos;
using ApartmentBooking.Application.Features.Common;
using ApartmentBooking.Application.UnitOfWork;
using ApartmentBooking.Domain.Entities;
using AutoMapper;
using System.Net;

namespace ApartmentBooking.Application.Features.Apartments.Queries
{
    public record GetApartmentDetailsQueryRequest(Guid id) : IRequest<ApiResponse<ApartmentDetailsDto>>;

    public class GetApartmentDetailsQueryHandler(IQueryUnitOfWork query, IMapper mapper) : IRequestHandler<GetApartmentDetailsQueryRequest, ApiResponse<ApartmentDetailsDto>>
    {
        private readonly IQueryUnitOfWork _query = query;
        private readonly IMapper _mapper = mapper;
        public async Task<ApiResponse<ApartmentDetailsDto>> Handle(GetApartmentDetailsQueryRequest request, CancellationToken cancellationToken)
        {
            var apartment = await _query.QueryRepository<Apartment>().GetWithIncludeAsync(false, x => x.Id == request.id, x => x.ApartmentAmenitiesAssociations!);
            _ = apartment ?? throw new Exception("Apartment not found");

            var apartmentDto = _mapper.Map<ApartmentDetailsDto>(apartment);
            
            if(apartment.ApartmentAmenitiesAssociations != null)
            {
                apartmentDto!.ApartmentAmenitiesAssociation = apartment.ApartmentAmenitiesAssociations.Select(x => x.AmenitiesId).ToList();
                apartmentDto.Amenities = await _query.AmenitiesQuery.GetAmenitiesName(apartmentDto.ApartmentAmenitiesAssociation, cancellationToken);
            }

            var response = new ApiResponse<ApartmentDetailsDto>
            {
                Success = apartmentDto != null,
                StatusCode = apartmentDto != null ? (int)HttpStatusCode.OK : (int)HttpStatusCode.BadRequest,
                Data = apartmentDto!,
                Message = apartmentDto != null ? "Apartment data found" : "Apartment data not found"
            };

            return response;
        }
    }
}
