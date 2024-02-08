using ApartmentBooking.Domain.Common.Contracts;
using Microsoft.AspNetCore.Identity;

namespace ApartmentBooking.Identity.Models
{
    public class ApplicationUser  : IdentityUser, IAuditableEntity, ISoftDelete
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? ImageUrl { get; set; }
        public string? RefreshToken { get; set; }
        public DateTime RefreshTokenExpiryTime { get; set; }
        public string? Culture { get; set; }
        public string? VerificationCode { get; set; }
        public bool IsActive { get; set; } = true;
        public string? CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string? ModifiedBy { get; set; }
        public DateTime ModifiedOn { get; set; }
        public string? DeletedBy { get; set; }
        public DateTime DeletedOn { get; set; }
        public bool IsDeleted { get; set; } = false;
        public Guid InvitedBy { get; set; }
        public DateTime InvitedDate { get; set; }
        public bool? IsSuperAdmin { get; set; } = false;
    }
}
