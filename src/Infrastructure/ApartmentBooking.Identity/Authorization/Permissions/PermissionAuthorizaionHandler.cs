using ApartmentBooking.Application.Contracts.Identity;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace ApartmentBooking.Identity.Authorization.Permissions
{
    public class PermissionAuthorizaionHandler(IUsersService usersService) : AuthorizationHandler<PermissionRequirement>
    {
        private readonly IUsersService _usersService = usersService;

        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, PermissionRequirement requirement)
        {
            if(context.User.FindFirstValue("uid") is { } userId &&
                await _usersService.HasPermissionAsync(userId, requirement.Permission))
            {
                context.Succeed(requirement);
            }
        }
    }
}