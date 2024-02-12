using ApartmentBooking.Application.Contracts.Responses;
using ApartmentBooking.Application.Features.Common;
using ApartmentBooking.Application.Interfaces;
using ApartmentBooking.Application.Model.Users;

namespace ApartmentBooking.Application.Contracts.Identity
{
    public interface IUsersService : ITransientService
    {
        Task<ApiResponse<string>> UpdateAsync(UpdateUserDto user);
        Task<ApiResponse<string>> DeleteAsync(string userId);
        Task<ApiResponse<UserDetailsDto>> GetUserDetails(string userId, CancellationToken cancellationToken);
        Task<IPagedDataResponse<UserListDto>> SearchAsync(UserListFilter flter, CancellationToken cancellationToken);
        Task<List<string>> GetPermissionAsync(string userId, CancellationToken cancellationToken);
        Task<bool> HasPermissionAsync(string? userId, string permission, CancellationToken cancellationToken = default);
    }
}
