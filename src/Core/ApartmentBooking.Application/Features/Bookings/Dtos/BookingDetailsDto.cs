namespace ApartmentBooking.Application.Features.Bookings.Dtos
{
    public class BookingDetailsDto
    {
        public Guid Id { get; set; }
        public DateTime BookFrom { get; set; }
        public DateTime BookTill { get; set; }
        public string ApartmentName { get; set; }
        public List<Guid>? Amenities { get; set; }
        public List<string>? AmenitiesName { get; set; }
        public string BookedBy { get; set; }
        public bool IsOnLease { get; set; }
        public string? LeaseDurationTime { get; set; }
    }
}
