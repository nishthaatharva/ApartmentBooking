namespace ApartmentBooking.Application.Model.Authentication
{
    public record RefreshTokenRequest(string Token, string RefreshToken);
}
