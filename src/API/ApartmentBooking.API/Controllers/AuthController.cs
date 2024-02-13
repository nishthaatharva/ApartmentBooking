using ApartmentBooking.API.Controllers.Base;
using ApartmentBooking.Application.Contracts.Identity;
using ApartmentBooking.Application.Contracts.Responses;
using ApartmentBooking.Application.Model.Authentication;
using Microsoft.AspNetCore.Mvc;

namespace ApartmentBooking.API.Controllers;

public class AuthController(IAuthService authService) : BaseApiController
{
    private readonly IAuthService _authService = authService;

    [HttpPost("signin")]
    public async Task<IResponse> SignInAsync(AuthenticationRequest request)
    {
        return await _authService.AuthenticateAsync(request);
    }

    [HttpPost("Register")]
    public async Task<ActionResult<RegistrationResponse>> RegisterAsync(RegistrationRequest request)
    {
        return Ok(await _authService.RegisterAsync(request));
    }

    [HttpPost("refreshToken")]
    public async Task<ActionResult<RefreshTokenResponse>> RefreshTokenAsync(RefreshTokenRequest request)
    {
        return Ok(await _authService.RefreshTokenAsync(request));
    }
}
