using ApartmentBooking.Application.Contracts.Application;
using ApartmentBooking.Application.Contracts.Caching;
using ApartmentBooking.Application.Contracts.Identity;
using ApartmentBooking.Application.Contracts.Responses;
using ApartmentBooking.Application.Features.Common;
using ApartmentBooking.Application.Model.Users;
using ApartmentBooking.Identity.Constants;
using ApartmentBooking.Identity.Data;
using ApartmentBooking.Identity.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Net;

namespace ApartmentBooking.Identity.Services
{
    public class UsersService(
        UserManager<ApplicationUser> userManager,
        IConfiguration configuration,
        RoleManager<ApplicationRole> roleManager,
        AppIdentityDbContext db,
        ICacheService cache,
        ICacheKeyService cacheKey,
        ICurrentUserService currentUserService) : IUsersService
    {
        private readonly UserManager<ApplicationUser> _userManager = userManager;
        private readonly RoleManager<ApplicationRole> _roleManager = roleManager;
        private readonly IConfiguration _configuration = configuration;
        private readonly AppIdentityDbContext _db = db;
        private readonly ICacheService _cache = cache;
        private readonly ICacheKeyService _cacheKey = cacheKey;
        private readonly ICurrentUserService _currentUserService = currentUserService;


        public async Task<ApiResponse<string>> UpdateAsync(UpdateUserDto request)
        {
            var users = await _userManager.FindByIdAsync(request.Id);
            _ = users ?? throw new Exception($"User not found");

            var existingEmail = await _userManager.FindByEmailAsync(request.Email!);
            if (existingEmail != null && existingEmail.Id != request.Id)
            {
                throw new Exception($"Email {request.Email} already exist");
            }

             users.FirstName = request.FirstName;
             users.LastName = request.LastName;

            if(request.Email != users.Email)
            {
                users.Email = users.UserName = request.Email;
                users.NormalizedEmail = users.NormalizedUserName = request.Email!.ToUpper();
            }

            var result = await _userManager.UpdateAsync(users);

            if (!result.Succeeded)
            {
                throw new Exception($"Failed to update user: {string.Join(", ", result.Errors.Select(e => e.Description))}");
            }

            return new ApiResponse<string>
            {
                Success = result.Succeeded,
                Data = "User updated successfully.",
                StatusCode = result.Succeeded ? (int)HttpStatusCode.OK : (int)HttpStatusCode.BadRequest,
                Message = result.Succeeded ? "User updated" : "Failed to update user."
            };
        }

        public async Task<ApiResponse<string>> DeleteAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            _ = user ?? throw new Exception("User not found");

            if(user.IsSuperAdmin == true && user.Email == _configuration["AppSettings:UserEmail"])
            {
                throw new Exception("Not allowed to delete user");
            }

            user.NormalizedUserName = user.UserName = user.UserName + "_" + Guid.NewGuid().ToString(); 
            var result = await _userManager.DeleteAsync(user);

            return new ApiResponse<string>
            {
                Success = result.Succeeded,
                Data = "User deleted successfully",
                StatusCode = result.Succeeded ? (int)HttpStatusCode.OK : (int)HttpStatusCode.BadRequest,
                Message = result.Succeeded ? "User deleted" : "Failed to delete user."
            };
        }

        public async Task<ApiResponse<UserDetailsDto>> GetUserDetails(string userId, CancellationToken cancellationToken)
        {
            var user = await (from u in _db.Users.AsNoTracking()
                              join ur in _db.UserRoles.AsNoTracking() on u.Id equals ur.UserId
                              join r in _db.Roles.AsNoTracking() on ur.RoleId equals r.Id
                              where u.Id == userId
                              select new UserDetailsDto()
                              {
                                  Id = u.Id,
                                  FirstName = u.FirstName ?? string.Empty,
                                  LastName = u.LastName ?? string.Empty,
                                  Email = u.Email ?? string.Empty,
                                  RoleId = r.Id, 
                                  RoleName = r.Name ?? string.Empty,
                              }).FirstOrDefaultAsync();
            _ = user ?? throw new Exception("User not found");

            var response = new ApiResponse<UserDetailsDto>
            {
                Success = user != null,
                StatusCode = user != null ? (int)HttpStatusCode.OK : (int)HttpStatusCode.BadRequest,
                Data = user!,
                Message = user != null ? "user data found" : "user data not found"
            };

            return response;
        }

        public async Task<IPagedDataResponse<UserListDto>> SearchAsync(UserListFilter filter, CancellationToken cancellationToken)
        {
            var usersList = (from u in _db.Users.AsNoTracking()
                             join ur in _db.UserRoles.AsNoTracking() on u.Id equals ur.UserId
                             join r in _db.Roles.AsNoTracking() on ur.RoleId equals r.Id
                             where u.Id != _currentUserService.UserId
                             select new UserListDto()
                             {
                                 Id = u.Id,
                                 FirstName = u.FirstName ?? string.Empty,
                                 LastName = u.LastName ?? string.Empty,
                                 Email = u.Email ?? string.Empty,
                                 FullName = u.FirstName + " " + u.LastName,
                                 RoleId = r.Id,
                                 Role = r.Name ?? string.Empty,
                                 CreatedOn = u.CreatedOn
                             }
                            );
            var spec = new GetSearchUserRequestSpec(filter);

            var users = await usersList.ApplySpecification(spec);
            int count = await usersList.ApplySpecificationCount(spec);

            return new PagedApiResponse<UserListDto>(count, filter.PageNumber, filter.PageSize) { Data = users };
        }

        public async Task<List<string>> GetPermissionAsync(string userId, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByIdAsync(userId);
            _ = user ?? throw new Exception("Authentication Failed.");

            var userRoles = await _userManager.GetRolesAsync(user);
            var permissions = new List<string>();
            foreach (var roles in await _roleManager.Roles.
                Where(r => userRoles.Contains(r.Name!)).ToListAsync(cancellationToken))
            {
                permissions.AddRange(await _db.RoleClaims
                    .Where(rc => rc.RoleId == roles.Id && rc.ClaimType == IdentityRoleClaims.Permission)
                    .Select(rc => rc.ClaimValue!)
                    .ToListAsync(cancellationToken));
            }

            return permissions.Distinct().ToList();
        }

        public async Task<bool> HasPermissionAsync(string? userId, string permission, CancellationToken cancellationToken)
        {
            var permissions = await _cache.GetOrSetAsync(
            _cacheKey.GetCacheKey(IdentityRoleClaims.Permission, userId),
            () => GetPermissionAsync(userId, cancellationToken),
            TimeSpan.FromDays(1),
            cancellationToken: cancellationToken);

            return permissions?.Contains(permission) ?? false;
        }
    }
}
