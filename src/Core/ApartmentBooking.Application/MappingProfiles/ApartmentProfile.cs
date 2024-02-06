using ApartmentBooking.Application.Features.Apartments.Commands;
using ApartmentBooking.Domain.Entities;
using AutoMapper;

namespace ApartmentBooking.Application.MappingProfiles
{
    public class ApartmentProfile : Profile
    {
        public ApartmentProfile()
        {
            CreateMap<CreateApartmentCommandRequest, Apartment>()
                .ForMember(x => x.ApartmentAmenitiesAssociations, y => y.Ignore());
        }
    }
}
