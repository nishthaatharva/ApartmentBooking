namespace ApartmentBooking.Domain.Entities
{
    public class ApartmentAmenitiesAssociation : BaseAuditableEntity
    {
        public Guid ApartmentId { get; set; }
        public Apartment? Apartment { get; set; }
        public Guid AmenitiesId { get; set; }
        public Amenities? Amenities { get; set; }
    }
}
