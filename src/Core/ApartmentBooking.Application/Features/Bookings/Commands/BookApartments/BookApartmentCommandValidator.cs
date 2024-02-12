using ApartmentBooking.Application.UnitOfWork;

namespace ApartmentBooking.Application.Features.Bookings.Commands
{
    public class BookApartmentCommandValidator : AbstractValidator<BookApartmentCommandRequest>
    {
        private readonly IQueryUnitOfWork _query;

        public BookApartmentCommandValidator(IQueryUnitOfWork query)
        {
            _query = query;

            RuleFor(p => p.BookFrom)
                .NotEmpty()
                .WithMessage((_, name) => "Book from is required");

            RuleFor(p => p.BookTill)
                .NotEmpty()
                .WithMessage((_, name) => "Book till is required");

            RuleFor(p => p.ApartmentId)
                .NotEmpty()
                .WithMessage((_, name) => "Apartment Id is required");
        }
    }
}
