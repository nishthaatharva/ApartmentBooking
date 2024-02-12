using ApartmentBooking.Application.Contracts.Application;
using System.Security.Claims;

namespace ApartmentBooking.API.Services
{
    public class CurrentUserService(IHttpContextAccessor httpContextAccessor) : ICurrentUserService
    {
        private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;

        public string? UserId => _httpContextAccessor.HttpContext?.User?.FindFirstValue("uid");
        private ClaimsPrincipal? _user;
    }
}
