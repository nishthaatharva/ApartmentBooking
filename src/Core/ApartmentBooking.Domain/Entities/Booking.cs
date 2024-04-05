namespace ApartmentBooking.Domain.Entities
{
    public sealed class Booking : BaseAuditableEntity
    {
        public DateTime BookFrom { get; set; }
        public DateTime BookTill { get; set; }
        public bool IsOnLease { get; set; } = false;
        public int? LeaseDuration { get; set; } //1.week 2.month 3.year
        public Guid ApartmentId { get; set; }
        public Apartment? Apartment { get; set; }
        public bool IsBook { get; set; } = true;
    }
}
