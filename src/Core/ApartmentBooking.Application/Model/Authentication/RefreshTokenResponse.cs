namespace ApartmentBooking.Application.Model.Authentication
{
    public record RefreshTokenResponse(string Token, string RefreshToken, DateTime RefreshTokenExpiryTime);
}
