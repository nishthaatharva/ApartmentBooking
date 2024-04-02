using ApartmentBooking.Application.Contracts.Application;
using ApartmentBooking.Application.Contracts.Responses;
using ApartmentBooking.Application.Features.Bookings.Dtos;
using ApartmentBooking.Application.Model.Specification;
using ApartmentBooking.Application.Model.Specification.Filters;
using ApartmentBooking.Application.UnitOfWork;
using Ardalis.Specification;

namespace ApartmentBooking.Application.Features.Bookings.Queries
{
    public sealed record GetSearchMyBookingsQuery : IRequest<IPagedDataResponse<BookingListDto>>
    {
        public PaginationFilter PaginationFilter { get; set; } = default!;
    }

    public class GetSearchMyBookingSpec : EntitiesByPaginationFilterSpec<BookingListDto>
    {
        public GetSearchMyBookingSpec(GetSearchMyBookingsQuery request)
            : base(request.PaginationFilter) =>
            Query.OrderByDescending(c => c.Id, !request.PaginationFilter.HasOrderBy());
    }

    public class SearchMyBookingQueryHandler(IQueryUnitOfWork query, ICurrentUserService currentUserService) : IRequestHandler<GetSearchMyBookingsQuery, IPagedDataResponse<BookingListDto>>
    {
        private readonly IQueryUnitOfWork _query = query;
        private readonly ICurrentUserService _currentUserService = currentUserService;

        public async Task<IPagedDataResponse<BookingListDto>> Handle(GetSearchMyBookingsQuery request, CancellationToken cancellationToken)
        {
            var currentUser = _currentUserService.UserId;
            var spec = new GetSearchMyBookingSpec(request);

            return await _query.BookingQuery.SearchAsync(spec, request.PaginationFilter.PageNumber, request.PaginationFilter.PageSize, cancellationToken, currentUser);
        }
    }
}
