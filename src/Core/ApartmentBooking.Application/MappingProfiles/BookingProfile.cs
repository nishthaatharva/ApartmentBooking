using ApartmentBooking.Application.Features.Bookings.Commands;
using ApartmentBooking.Domain.Entities;
using AutoMapper;

namespace ApartmentBooking.Application.MappingProfiles
{
    public class BookingProfile : Profile
    {
        public BookingProfile()
        {
            CreateMap<BookApartmentCommandRequest, Booking>();
        }
    }
}
