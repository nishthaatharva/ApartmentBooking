using ApartmentBooking.Application.Contracts.Responses;
using ApartmentBooking.Application.Features.Apartments.Dtos;
using ApartmentBooking.Application.Model.Specification;
using ApartmentBooking.Application.Model.Specification.Filters;
using ApartmentBooking.Application.UnitOfWork;
using Ardalis.Specification;

namespace ApartmentBooking.Application.Features.Apartments.Queries
{
    public sealed record SearchApartmentQueryRequest : IRequest<IPagedDataResponse<ApartmentListDto>>
    {
        public PaginationFilter PaginationFilter { get; set; } = default!;
        public List<string> AmenitiesId { get; set; } = default!;
    }

    public class GetApartmentRequestSpec : EntitiesByPaginationFilterSpec<ApartmentListDto>
    {
        public GetApartmentRequestSpec(SearchApartmentQueryRequest request) 
            :base(request.PaginationFilter) =>
            Query.OrderByDescending(c => c.Id, !request.PaginationFilter.HasOrderBy());
    }

    public class SearchApartmentQueryHandler(IQueryUnitOfWork query) : IRequestHandler<SearchApartmentQueryRequest, IPagedDataResponse<ApartmentListDto>>
    {
        private readonly IQueryUnitOfWork _query = query;

        public async Task<IPagedDataResponse<ApartmentListDto>> Handle(SearchApartmentQueryRequest request, CancellationToken cancellationToken)
        {
            var spec = new GetApartmentRequestSpec(request);

            return await _query.ApartmentQuery.SearchAsync(spec, request.PaginationFilter.PageNumber, request.PaginationFilter.PageSize, request.AmenitiesId, cancellationToken);
        }
    }
}
