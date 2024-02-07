using ApartmentBooking.Application.Features.Apartments.Commands;
using ApartmentBooking.Application.Features.Apartments.Dtos;
using ApartmentBooking.Domain.Entities;
using ApartmentBooking.Domain.Enums;
using AutoMapper;

namespace ApartmentBooking.Application.MappingProfiles
{
    public class ApartmentProfile : Profile
    {
        public ApartmentProfile()
        {
            CreateMap<CreateApartmentCommandRequest, Apartment>()
                .ForMember(x => x.ApartmentAmenitiesAssociations, y => y.Ignore());

            CreateMap<Apartment, ApartmentDetailsDto>()
                .ForMember(x => x.StatusName, opt => opt.MapFrom(x => ((Status)x.Status).ToString()))
                .ForMember(x => x.ApartmentAmenitiesAssociation, y => y.Ignore());
        }
    }
}
