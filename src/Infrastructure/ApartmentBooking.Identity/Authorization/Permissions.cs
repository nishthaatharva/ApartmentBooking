using System.Collections.ObjectModel;

namespace ApartmentBooking.Identity.Authorization
{
    public class AllPermissions
    {
        private static readonly Permission[] _all =
        [
            new("Create User", Action.Create, Resource.Users, IsAdmin: true),
            new("Update User", Action.Update, Resource.Users, IsAdmin: true),
            new("Delete User", Action.Delete, Resource.Users, IsAdmin: true),
            new("View User", Action.View, Resource.Users, IsAdmin: true),
            new("Search User", Action.Search, Resource.Users, IsAdmin: true),
            new("Create Apartment", Action.Create, Resource.Apartment),
            new("Update Apartment", Action.Update, Resource.Apartment),
            new("Delete Apartment", Action.Delete, Resource.Apartment),
            new("View Apartment", Action.View, Resource.Apartment),
            new("Search Apartment", Action.Search, Resource.Apartment),
            new("Book Apartment", Action.Create, Resource.Booking, IsAdmin: true, IsBasic: true),
        ];

        public static IReadOnlyList<Permission> All { get; } = new ReadOnlyCollection<Permission>(_all);
        public static IReadOnlyList<Permission> Root { get; } = new ReadOnlyCollection<Permission>(_all.Where(p => p.IsRoot || p.IsAdmin || p.IsBasic).ToArray());
        public static IReadOnlyList<Permission> Admin { get; } = new ReadOnlyCollection<Permission>(_all.Where(p => p.IsRoot || p.IsAdmin || p.IsBasic).ToArray());
        public static IReadOnlyList<Permission> User { get; } = new ReadOnlyCollection<Permission>(_all.Where(p => p.IsBasic).ToArray());

    }

    public record Permission(string Description, string Action, string Resource, bool IsAdmin = false, bool IsBasic = false, bool IsRoot = false)
    {
        public string Name => NameFor(Action, Resource);
        public static string NameFor(string action, string resource) => $"Permissions.{resource}.{action}";
    }
}
