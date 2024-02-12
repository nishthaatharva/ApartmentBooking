namespace ApartmentBooking.Application.Contracts.Application
{
    public interface ICurrentUserService
    {
        string? UserId { get; }
    }
}
