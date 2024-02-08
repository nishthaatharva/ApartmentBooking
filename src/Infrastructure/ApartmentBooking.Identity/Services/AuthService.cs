using ApartmentBooking.Application.Contracts.Identity;
using ApartmentBooking.Application.Contracts.Responses;
using ApartmentBooking.Application.Features.Common;
using ApartmentBooking.Application.Model.Authentication;
using ApartmentBooking.Identity.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace ApartmentBooking.Identity.Services
{
    public class AuthService(
        UserManager<ApplicationUser> userManager,
        SignInManager<ApplicationUser> signInManager,
        IOptions<JwtSettings> jwtSettings) : IAuthService
    {
        private readonly UserManager<ApplicationUser> _userManager = userManager;
        private readonly SignInManager<ApplicationUser> _signInManager = signInManager;
        private readonly JwtSettings _jwtSettings = jwtSettings.Value;

        #region Public methods
        public async Task<IResponse> AuthenticateAsync(AuthenticationRequest request)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);

            _ = user ?? throw new Exception($"User {request.Email} not found");

            var result = await _signInManager.PasswordSignInAsync(user?.UserName!, request.Password, false, lockoutOnFailure: false);
            if(!result.Succeeded)
            {
                throw new Exception("Please provide valid credentials");
            }

            JwtSecurityToken jwtSecurityToken = await GenerateToken(user!);

#pragma warning disable CS8602 // Dereference of a possibly null reference.
            user.RefreshToken = GenerateRefreshToken();
            user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(_jwtSettings.RefreshTokenExpirationInDays);
            await _userManager.UpdateAsync(user);

            AuthenticationResponse response = new AuthenticationResponse
            {
                Id = user?.Id!,
                Token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken),
                RefreshToken = user.RefreshToken,
                Email = user?.Email!,
                UserName = user?.UserName!
            };
#pragma warning restore CS8602 // Dereference of a possibly null reference.

            return new ApiResponse<AuthenticationResponse>
            {
                Data = response,
                StatusCode = (int)HttpStatusCode.OK,
                Success = true,
                Message = "Logged in successfully."
            };
        }

        #endregion


        #region Private methods

        private async Task<JwtSecurityToken> GenerateToken(ApplicationUser user)
        {
            var userClaims = await _userManager.GetClaimsAsync(user);
            var roles = await _userManager.GetRolesAsync(user);

            var roleClaims = new List<Claim>();

            for(int i = 0;  i < roles.Count; i++)
            {
                roleClaims.Add(new Claim("roles", roles[i]));
            }

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Name, user.FirstName+" "+ user.LastName),
                new Claim(JwtRegisteredClaimNames.Sub, user.UserName!),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email!),
                new Claim("uid", user.Id)
            }
            .Union(userClaims)
            .Union(roleClaims);

            var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Key));
            var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);

            var jwtSecurityToken = new JwtSecurityToken(
                issuer: _jwtSettings.Issuer,
                audience: _jwtSettings.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(_jwtSettings.DurationInMinutes),
                signingCredentials: signingCredentials);
            return jwtSecurityToken;
        }

        private string GenerateRefreshToken()
        {
            var numbers = new byte[32];
            using RandomNumberGenerator randomNumber = RandomNumberGenerator.Create();
            randomNumber.GetBytes(numbers);
            return Convert.ToBase64String(numbers);
        }

        #endregion
    }
}
