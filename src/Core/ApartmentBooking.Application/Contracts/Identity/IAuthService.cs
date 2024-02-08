using ApartmentBooking.Application.Contracts.Responses;
using ApartmentBooking.Application.Interfaces;
using ApartmentBooking.Application.Model.Authentication;

namespace ApartmentBooking.Application.Contracts.Identity
{
    public interface IAuthService : ITransientService
    {
        Task<IResponse> AuthenticateAsync(AuthenticationRequest request);
    }
}
