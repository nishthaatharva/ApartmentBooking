using System.Collections.ObjectModel;

namespace ApartmentBooking.Identity.Authorization
{
    public class AllPermissions
    {
        private static readonly Permission[] _all =
        [
            
        ];

        public static IReadOnlyList<Permission> All { get; } = new ReadOnlyCollection<Permission>(_all);
        public static IReadOnlyList<Permission> Root { get; } = new ReadOnlyCollection<Permission>(_all.Where(p => p.IsRoot || p.IsAdmin || p.IsBasic).ToArray());
        public static IReadOnlyList<Permission> Admin { get; } = new ReadOnlyCollection<Permission>(_all.Where(p => !p.IsRoot).ToArray());
        public static IReadOnlyList<Permission> User { get; } = new ReadOnlyCollection<Permission>(_all.Where(p => p.IsAdmin || p.IsBasic).ToArray());

    }

    public record Permission(string Description, string Action, string Resource, bool IsAdmin = false, bool IsBasic = false, bool IsRoot = false)
    {
        public string Name => NameFor(Action, Resource);
        public static string NameFor(string action, string resource) => $"Permissions.{resource}.{action}";
    }
}
