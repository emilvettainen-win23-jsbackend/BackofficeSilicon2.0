using Infrastructure.Data.Entities;
using Infrastructure.Dtos;
using Infrastructure.Factories;
using Infrastructure.Helpers;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Services;

public class UserService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly ILogger<UserService> _logger;
    private readonly RoleManager<IdentityRole> _roleManager;

    private readonly AuthenticationStateProvider _authenticationStateProvider;

    public UserService(UserManager<ApplicationUser> userManager, ILogger<UserService> logger, RoleManager<IdentityRole> roleManager, AuthenticationStateProvider authenticationStateProvider)
    {
        _userManager = userManager;
        _logger = logger;
        _roleManager = roleManager;
        _authenticationStateProvider = authenticationStateProvider;
    }

    public async Task<UserDto?> GetOneUserAsync(string userId)
    {
        try
        {
            var user = await _userManager.FindByIdAsync(userId);

            if (user != null)
            {
                var userDto = new UserDto
                {
                    Id = user.Id,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Email = user.Email!,
                    //ConfirmPassword = "",
                    Created = user.Created,
                    Updated = user.Updated
                };

                var roles = await _userManager.GetRolesAsync(user);
                userDto.Role = string.Join(",", roles);

                return userDto;
            }

            return null;
        }
        catch (Exception ex)
        {
            _logger.LogError($"ERROR: UserService.GetOneUserAsync() :: {ex.Message}");
            return null;
        }
    }


    public async Task<ResponseResult> CreateUserAsync(ApplicationUser user, string password, string role)
    {
        try
        {

            var userExists = await _userManager.Users.AnyAsync(x => x.Email == user.Email);
            if (userExists)
            {
                return ResponseFactory.Exists();
            }

            var createUser = await _userManager.CreateAsync(user, password);

            if (!createUser.Succeeded)
            {
                return ResponseFactory.Error();
            }

            var addToRoleResult = await _userManager.AddToRoleAsync(user, role);

            return createUser.Succeeded ? ResponseFactory.Ok() : ResponseFactory.Error();
        }
        catch (Exception ex)
        {
            _logger.LogError($"ERROR : UserService.CreateUserAsync() :: {ex.Message}");
            return ResponseFactory.Error();
        }
    }

    public async Task<ResponseResult> ConfirmEmailAsync(ApplicationUser user)
    {
        try
        {
            user.Updated = DateTime.Now;
            var updateResult = await _userManager.UpdateAsync(user);

            return updateResult.Succeeded ? ResponseFactory.Ok() : ResponseFactory.Error();
        }
        catch (Exception ex)
        {
            _logger.LogError($"ERROR: UserService.UpdateUserAsync() :: {ex.Message}");
            return ResponseFactory.ServerError();
        }
    }

    public async Task<List<AllUsersDto>> GetAllUsersAsync()
    {
        try
        {
            var users = await _userManager.Users.ToListAsync();
            if (users != null)
            {
                var userDtos = users.Select(user => new AllUsersDto
                {
                    Id = user.Id,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Email = user.Email!
                }).ToList();

                return userDtos;
            }

            return new List<AllUsersDto>();
        }
        catch (Exception ex)
        {
            _logger.LogError($"ERROR: UserService.GetAllUsersAsync() :: {ex.Message}");
            return new List<AllUsersDto>();
        }
    }

    public async Task<bool> DeleteUserAsync(string userId)
    {
        try
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                _logger.LogWarning("User could not be found.");
                return false;
            }
            var result = await _userManager.DeleteAsync(user);
            if (result.Succeeded)
            {
                return true;
            }

            _logger.LogError("Could not delete user.");

            return false;
        }
        catch (Exception ex)
        {
            _logger.LogError($"ERROR: UserService.DeleteUserAsync() :: {ex.Message}");
            return false;
        }
    }

    public async Task<ResponseResult> UpdateUserAsync(UserDto userDto, string role)
    {
        try
        {
            var existingUser = await _userManager.FindByIdAsync(userDto.Id);

            if (existingUser == null)
            {
                return ResponseFactory.NotFound();
            }

            existingUser.FirstName = userDto.FirstName;
            existingUser.LastName = userDto.LastName;
            existingUser.Email = userDto.Email;
            existingUser.Updated = DateTime.Now;

            var existingRole = await _roleManager.FindByNameAsync(role);

            if (existingRole == null)
            {
                return ResponseFactory.Error(message: "Role not found.");
            }

            var removeRolesResult = await _userManager.RemoveFromRolesAsync(existingUser, await _userManager.GetRolesAsync(existingUser));

            if (!removeRolesResult.Succeeded)
            {
                return ResponseFactory.Error(message: "Failed to remove existing roles.");
            }

            var addToRoleResult = await _userManager.AddToRoleAsync(existingUser, role);

            if (!addToRoleResult.Succeeded)
            {
                return ResponseFactory.Error(message: "Failed to add role to user.");
            }

            var updateResult = await _userManager.UpdateAsync(existingUser);
            if (!updateResult.Succeeded)
            {
                return ResponseFactory.Error();
            }



            var passwordToken = await _userManager.GeneratePasswordResetTokenAsync(existingUser);
            var updatePassword = await _userManager.ResetPasswordAsync(existingUser, passwordToken , userDto.Password);



            return updatePassword.Succeeded ? ResponseFactory.Ok() : ResponseFactory.Error();
        }
        catch (Exception ex)
        {
            _logger.LogError($"ERROR: UserService.UpdateUserAsync() :: {ex.Message}");
            return ResponseFactory.ServerError();
        }
    }


}
