using ApartmentBooking.Application.Contracts.Infrastructure.Repositories.Apartments;
using ApartmentBooking.Application.Contracts.Responses;
using ApartmentBooking.Application.Extensions;
using ApartmentBooking.Application.Features.Apartments.Dtos;
using ApartmentBooking.Application.Features.Common;
using ApartmentBooking.Domain.Entities;
using ApartmentBooking.Domain.Enums;
using ApartmentBooking.Infrastructure.Data;
using ApartmentBooking.Infrastructure.Repositories.Base;
using ApartmentBooking.Infrastructure.Services;
using Ardalis.Specification;
using Microsoft.EntityFrameworkCore;

namespace ApartmentBooking.Infrastructure.Repositories.Apartments
{
    public class ApartmentQueryRepository : QueryRepository<Apartment>, IApartmentQueryRepository
    {
        public ApartmentQueryRepository(DataContext context) : base(context) { }

        public async Task<IPagedDataResponse<ApartmentListDto>> SearchAsync(ISpecification<ApartmentListDto> spec, int pageNumber, int pageSize, CancellationToken cancellationToken)
        {
            var apartmentList = (from c in _context.Apartments.AsNoTracking().Include(g => g.ApartmentAmenitiesAssociations)
                                 select new ApartmentListDto()
                                 {
                                     Id = c.Id,
                                     Name = c.Name, 
                                     Status = c.Status,
                                     StatusName = CommonFunction.GetEnumDisplayName((Status)c.Status),
                                     Rooms = c.Rooms,
                                     Location = c.Location,
                                     Size = c.Size,

                                     ApartmentAmenitiesAssociation = c.ApartmentAmenitiesAssociations!
                                            .Select(x => x.AmenitiesId.ToString()).ToList(),

                                     Amenities = _context.Amenities.Where(a => c.ApartmentAmenitiesAssociations!
                                            .Any(aa => aa.AmenitiesId == a.Id))
                                            .Select(a => a.Name).ToList()
                                 });
            var apartment = await apartmentList.ApplySpecification(spec);
            var count = await apartmentList.ApplySpecificationCount(spec);

            return new PagedApiResponse<ApartmentListDto>(count, pageNumber, pageSize) { Data = apartment };
        }
    }
}
