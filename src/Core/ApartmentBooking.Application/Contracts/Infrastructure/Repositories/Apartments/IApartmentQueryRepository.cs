using ApartmentBooking.Application.Contracts.Infrastructure.Repositories.Base;
using ApartmentBooking.Application.Contracts.Responses;
using ApartmentBooking.Application.Features.Apartments.Dtos;
using ApartmentBooking.Domain.Entities;
using Ardalis.Specification;

namespace ApartmentBooking.Application.Contracts.Infrastructure.Repositories.Apartments
{
    public interface IApartmentQueryRepository : IQueryRepository<Apartment>
    {
        Task<IPagedDataResponse<ApartmentListDto>> SearchAsync(ISpecification<ApartmentListDto> spec,
                                                            int pageNumber,
                                                            int pageSize,
                                                            CancellationToken cancellationToken);
    }
}
