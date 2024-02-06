namespace ApartmentBooking.Domain.Common
{
    public abstract class BaseAuditableEntity : BaseEntity
    {
        public DateTime CreatedOn { get; set; } = DateTime.UtcNow;
        public string? CreatedBy { get; set; }
        public DateTime ModifiedOn { get; set; } = DateTime.UtcNow;
        public string? ModifiedBy { get; set; }
        public DateTime DeletedOn { get; set; } = DateTime.UtcNow;
        public string? DeletedBy { get; set; }
        public bool IsDeleted { get; set; } = false;
    }
}
