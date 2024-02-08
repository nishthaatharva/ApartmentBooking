using Microsoft.AspNetCore.Identity;

namespace ApartmentBooking.Identity.Models
{
    public class ApplicationRole : IdentityRole
    {
        public string? Description { get; set; }
        public ApplicationRole(string name, string? description = null)
        : base(name)
        {
            Description = description;
            NormalizedName = name.ToUpperInvariant();
            CreatedOn = DateTime.UtcNow;
            ModifiedOn = DateTime.UtcNow;
        }
        public Guid CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; } = DateTime.UtcNow;
        public Guid ModifiedBy { get; set; }
        public DateTime? ModifiedOn { get; set; } = DateTime.UtcNow;
        public Guid? DeletedBy { get; set; }
        public DateTime? DeletedOn { get; set; } = DateTime.UtcNow;
        public bool IsActive { get; set; } = true;
        public bool IsDeleted { get; set; } = false;
    }
}
