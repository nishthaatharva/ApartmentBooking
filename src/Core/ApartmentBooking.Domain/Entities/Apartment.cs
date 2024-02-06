using ApartmentBooking.Domain.Common;

namespace ApartmentBooking.Domain.Entities
{
    public sealed class Apartment : BaseAuditableEntity
    {
        public string Location { get; set; }
        public int Size { get; set; }
        public int Rooms { get; set; }
        public int Amenities { get; set; } // 1.Pool, 2.Gym, 3.Garden
        public int Status { get; set; } // 1.Available, 2.Reserved
    }
}
