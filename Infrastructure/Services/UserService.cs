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

    private readonly AuthenticationStateProvider _authenticationStateProvider;

    public UserService(UserManager<ApplicationUser> userManager, ILogger<UserService> logger, AuthenticationStateProvider authenticationStateProvider)
    {
        _userManager = userManager;
        _logger = logger;
        _authenticationStateProvider = authenticationStateProvider;
    }

    public async Task<UserDto> GetCurrentUserAsync()
    {
        var authState = await _authenticationStateProvider.GetAuthenticationStateAsync();
        var user = authState.User;

        if (user.Identity == null || !user.Identity.IsAuthenticated)
        {
            return new UserDto();
        }

        var currentUser = await _userManager.GetUserAsync(user);
        return UserFactory.GetUser(currentUser!) ?? new UserDto();
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

    public async Task<ResponseResult> UpdateUserAsync(ApplicationUser user)
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

}
