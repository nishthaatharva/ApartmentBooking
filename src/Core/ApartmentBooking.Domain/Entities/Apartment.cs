namespace ApartmentBooking.Domain.Entities
{
    public sealed class Apartment : BaseAuditableEntity
    {
        public string Name { get; set; }
        public string Location { get; set; }
        public int Size { get; set; }
        public int Rooms { get; set; }
        public int Status { get; set; } // 1.Available, 2.Reserved
        public ICollection<ApartmentAmenitiesAssociation>? ApartmentAmenitiesAssociations { get; set; }
    }
}
