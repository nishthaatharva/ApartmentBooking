using ApartmentBooking.API.Controllers.Base;
using ApartmentBooking.Application.Contracts.Identity;
using ApartmentBooking.Application.Contracts.Responses;
using ApartmentBooking.Application.Features.Common;
using ApartmentBooking.Application.Model.Users;
using ApartmentBooking.Identity.Authorization;
using ApartmentBooking.Identity.Authorization.Permissions;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using Action = ApartmentBooking.Identity.Authorization.Action;


namespace ApartmentBooking.API.Controllers;

public class UsersController(IUsersService userService) : BaseApiController
{
    private readonly IUsersService _usersService = userService;

    [HttpPost("Search")]
    [MustHavePermission(Action.Search, Resource.Users)]
    public async Task<IPagedDataResponse<UserListDto>> GetSearchAsync(UserListFilter filter, CancellationToken cancellationToken)
    {
        return await _usersService.SearchAsync(filter, cancellationToken);
    }

    [HttpPut("{id}")]
    [MustHavePermission(Action.Update, Resource.Users)]
    public async Task<ApiResponse<string>> UpdateAsync(string id, UpdateUserDto request)
    {
        if(id != request.Id)
        {
            return new ApiResponse<string>
            {
                Success = false,
                Data = "The provided ID in the route does not match the ID in the request body.",
                StatusCode = (int)HttpStatusCode.BadRequest,
            };
        }
        return await _usersService.UpdateAsync(request);
    }

    [HttpDelete("{userId}")]
    [MustHavePermission(Action.Delete, Resource.Users)]
    public async Task<ApiResponse<string>> DeleteAsync(string userId)
    {
        return await _usersService.DeleteAsync(userId);
    }

    [HttpGet("{id}")]
    [MustHavePermission(Action.View, Resource.Users)]
    public async Task<ApiResponse<UserDetailsDto>> GetUserDetails(string id, CancellationToken cancellationToken)
    {
        return await _usersService.GetUserDetails(id, cancellationToken);
    }
}
