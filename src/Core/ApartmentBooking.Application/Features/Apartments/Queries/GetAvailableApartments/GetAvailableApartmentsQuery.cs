using ApartmentBooking.Application.Features.Apartments.Dtos;
using ApartmentBooking.Application.Features.Common;
using ApartmentBooking.Application.UnitOfWork;
using ApartmentBooking.Domain.Entities;
using AutoMapper;
using System.Net;

namespace ApartmentBooking.Application.Features.Apartments.Queries;

public record GetAvailableApartmentsQuery : IRequest<ApiResponse<List<ApartmentDetailsDto>>>;

public class GetAvailableApartmentsHandler(IQueryUnitOfWork query, IMapper mapper) : IRequestHandler<GetAvailableApartmentsQuery, ApiResponse<List<ApartmentDetailsDto>>>
{
    private readonly IQueryUnitOfWork _query = query;
    private readonly IMapper _mapper = mapper;
        
    public async Task<ApiResponse<List<ApartmentDetailsDto>>> Handle(GetAvailableApartmentsQuery request, CancellationToken cancellationToken)
    {
        var apartments = await _query.QueryRepository<Apartment>().GetAllWithIncludeAsync(false, x => x.Status == 1, x => x.ApartmentAmenitiesAssociations!);

        var apartmentDtos = _mapper.Map<List<ApartmentDetailsDto>>(apartments);

        foreach (var apartmentDto in apartmentDtos!)
        {
            if (apartments.Any(a => a.Id == apartmentDto.Id && a.ApartmentAmenitiesAssociations != null))
            {
                var apartment = apartments.First(a => a.Id == apartmentDto.Id);
                apartmentDto.ApartmentAmenitiesAssociation = apartment.ApartmentAmenitiesAssociations!.Select(x => x.AmenitiesId).ToList();
                apartmentDto.Amenities = await _query.AmenitiesQuery.GetAmenitiesName(apartmentDto.ApartmentAmenitiesAssociation, cancellationToken);
            }
        }
        
        return new ApiResponse<List<ApartmentDetailsDto>>
        {
            Success = apartmentDtos != null,
            StatusCode = apartmentDtos != null ? (int)HttpStatusCode.OK : (int)HttpStatusCode.BadRequest,
            Data = apartmentDtos!,
            Message = apartmentDtos != null ? "Available apartment data found" : "Failed to find apartment"
        };
    }
}