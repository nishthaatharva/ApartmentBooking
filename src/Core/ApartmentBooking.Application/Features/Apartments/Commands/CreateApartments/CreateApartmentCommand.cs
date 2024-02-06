using ApartmentBooking.Application.Features.Common;
using ApartmentBooking.Application.UnitOfWork;
using ApartmentBooking.Domain.Entities;
using AutoMapper;
using System.Net;

namespace ApartmentBooking.Application.Features.Apartments.Commands
{
    public sealed record CreateApartmentCommandRequest : IRequest<ApiResponse<string>>
    {
        public string Name { get; set; }
        public string Location { get; set; }
        public int Size { get; set; }
        public int Rooms { get; set; }
        public int Status { get; set; }
        public List<string>? ApartmentAmenitiesAssociation { get; set; }
    }

    public class CreateApartmentCommandHandler(ICommandUnitOfWork command, IMapper mapper) : IRequestHandler<CreateApartmentCommandRequest, ApiResponse<string>>
    {
        private readonly ICommandUnitOfWork _command = command;
        private readonly IMapper _mapper = mapper;
        public async Task<ApiResponse<string>> Handle(CreateApartmentCommandRequest request, CancellationToken cancellationToken)
        {
            var apartment = _mapper.Map<Apartment>(request);

            if (request.ApartmentAmenitiesAssociation != null)
            {
                apartment!.ApartmentAmenitiesAssociations = request.ApartmentAmenitiesAssociation
                .Select(item => new ApartmentAmenitiesAssociation()
                {
                    AmenitiesId = Guid.Parse(item),
                    ApartmentId = apartment.Id
                }).ToList();
            }

            await _command.CommandRepository<Apartment>().AddAsync(apartment);
            var result = await _command.SaveAsync(cancellationToken);
            var response = new ApiResponse<string>
            {
                Success = result > 0,
                StatusCode = result > 0 ? (int)HttpStatusCode.Created : (int)HttpStatusCode.BadRequest,
                Data = "Apartment added successfully",
                Message = result > 0 ? "Apartment created" : "Failed to create apartment"
            };
            return response;
        }
    }
}
