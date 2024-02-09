using ApartmentBooking.Application.Model.Specification;
using ApartmentBooking.Application.Model.Specification.Filters;
using Ardalis.Specification;

namespace ApartmentBooking.Application.Model.Users
{
    public class UserListFilter : PaginationFilter
    {
    }

    public class GetSearchUserRequestSpec : EntitiesByPaginationFilterSpec<UserListDto> 
    {
        public GetSearchUserRequestSpec(UserListFilter request)
            :base(request) =>
                Query.OrderByDescending(u => u.Id, !request.HasOrderBy());
    }
}
