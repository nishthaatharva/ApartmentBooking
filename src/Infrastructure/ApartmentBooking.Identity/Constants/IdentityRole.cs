using ApartmentBooking.Domain.Constant;
using System.Collections.ObjectModel;

namespace ApartmentBooking.Identity.Constants
{
    public class IdentityRole
    {
        public const string Administrator = nameof(Roles.Administrator);
        public const string User = nameof(Roles.User);

        public static IReadOnlyList<string> DefaultRoles { get; } = new ReadOnlyCollection<string>(new[]
        {
            Administrator, User
        });

        public static bool IsDefault(string roleName) => DefaultRoles.Any(r => r ==  roleName);
    }
}
