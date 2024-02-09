using ApartmentBooking.Application.Contracts.Responses;
using ApartmentBooking.Application.Interfaces;
using ApartmentBooking.Application.Model.Authentication;

namespace ApartmentBooking.Application.Contracts.Identity
{
    public interface IAuthService : ITransientService
    {
        Task<IResponse> AuthenticateAsync(AuthenticationRequest request);
        Task<RegistrationResponse> RegisterAsync(RegistrationRequest request);
        Task<RefreshTokenResponse> RefreshTokenAsync(RefreshTokenRequest request);
        Task<bool> AuthorizeAsync(string userId, string policyName);
    }
}
