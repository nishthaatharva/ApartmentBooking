using ApartmentBooking.Application.UnitOfWork;
using ApartmentBooking.Domain.Entities;

namespace ApartmentBooking.Application.Features.Apartments.Commands
{
    public class CreateApartmentCommandValidator : AbstractValidator<CreateApartmentCommandRequest>
    {
        private readonly IQueryUnitOfWork _query;

        public CreateApartmentCommandValidator(IQueryUnitOfWork query)
        {
            _query = query;

            RuleFor(p => p.Name)
            .NotEmpty()
            .MustAsync(async (apartment, name, ct) => await _query.QueryRepository<Apartment>().GetAsync(c => c.Name!.ToLower() == name!.ToLower()) is null)
            .WithMessage((_, name) => $"Apartment {name} already exists!");

            RuleFor(p => p.Location)
                .NotEmpty()
                .WithMessage((_, name) => "Location is required");

            RuleFor(p => p.Size)
                .NotEmpty()
                .WithMessage((_, name) => "Apartment size is required");

            RuleFor(p => p.Rooms)
                .NotEmpty()
                .WithMessage((_, name) => "Number of rooms in apartment is required");

            RuleFor(p => p.Status)
                .NotEmpty()
                .WithMessage((_, name) => "Status of apartment is required");
        }
    }
}
