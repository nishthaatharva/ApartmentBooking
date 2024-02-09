using ApartmentBooking.Application.Contracts.Identity;
using ApartmentBooking.Application.Contracts.Responses;
using ApartmentBooking.Application.Features.Common;
using ApartmentBooking.Application.Model.Users;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace ApartmentBooking.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController(IUsersService userService) : ControllerBase
    {
        private readonly IUsersService _usersService = userService;

        [HttpPost("Search")]
        public async Task<IPagedDataResponse<UserListDto>> GetSearchAsync(UserListFilter filter, CancellationToken cancellationToken)
        {
            return await _usersService.SearchAsync(filter, cancellationToken);
        }

        [HttpPut("{id}")] 
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
        public async Task<ApiResponse<string>> DeleteAsync(string userId)
        {
            return await _usersService.DeleteAsync(userId);
        }

        [HttpGet("{id}")]
        public async Task<ApiResponse<UserDetailsDto>> GetUserDetails(string id, CancellationToken cancellationToken)
        {
            return await _usersService.GetUserDetails(id, cancellationToken);
        }
    }
}
