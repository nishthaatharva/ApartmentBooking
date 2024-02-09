using Microsoft.AspNetCore.Authorization;

namespace ApartmentBooking.Identity.Authorization.Permissions
{
    public sealed class MustHavePermissionAttribute : AuthorizeAttribute
    {
        public MustHavePermissionAttribute(string action, string resource) =>
        Policy = Permission.NameFor(action, resource);
    }
}
