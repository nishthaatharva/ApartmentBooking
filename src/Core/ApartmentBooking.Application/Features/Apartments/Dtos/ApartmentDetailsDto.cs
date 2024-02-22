namespace ApartmentBooking.Application.Features.Apartments.Dtos
{
    public class ApartmentDetailsDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Location { get; set; }
        public int Size { get; set; }
        public int Rooms { get; set; }
        public int Status { get; set; }
        public string StatusName { get; set; }
        public List<Guid>? ApartmentAmenitiesAssociation { get; set; }
        public List<string>? Amenities { get; set; }
    }
}
