namespace ApartmentBooking.Application.Features.Bookings.Dtos
{
    public class BookingListDto
    {
        public Guid Id { get; set; }
        public DateTime BookFrom { get; set; }
        public DateTime BookTill { get; set; }
        public string BookFromDisplay { get; set; }
        public string BookTillDisplay { get; set; }
        public string ApartmentName { get; set; }
        public int Status { get; set; }
        public bool IsOnLease { get; set; }
        public bool IsBook { get; set; }
    }
}
