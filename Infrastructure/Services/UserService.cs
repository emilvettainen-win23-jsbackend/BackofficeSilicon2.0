
using Azure.Messaging.ServiceBus;
using Infrastructure.Data.Entities;
using Infrastructure.Dtos;
using Infrastructure.Factories;
using Infrastructure.Helpers;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Text;

namespace Infrastructure.Services;

public class UserService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly ILogger<UserService> _logger;
    private readonly HttpClient _httpClient;
    private readonly IConfiguration _config;
    private readonly AuthenticationStateProvider _authenticationStateProvider;
    private readonly ServiceBusClient _serviceBusClient;

    public UserService(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, ILogger<UserService> logger, AuthenticationStateProvider authenticationStateProvider, HttpClient httpClient, IConfiguration config)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _logger = logger;
        _authenticationStateProvider = authenticationStateProvider;
        _httpClient = httpClient;
        _config = config;
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

    public async Task<ResponseResult> CreateUserAsync(ApplicationUser user, string password)
    {
        try
        {

            var userExists = await _userManager.Users.AnyAsync(x => x.Email == user.Email);
            if (userExists)
            {
                return ResponseFactory.Exists();
            }

            var createUser = await _userManager.CreateAsync(user, password);
            return createUser.Succeeded ? ResponseFactory.Ok() : ResponseFactory.Error();
        }
        catch (Exception ex)
        {
            _logger.LogError($"ERROR : UserService.CreateUserAsync() :: {ex.Message}");
            return ResponseFactory.Error();
        }
    }

    public async Task<ResponseResult> UpdateUserAsync(string userId, ApplicationUser dto)
    {
        try
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return ResponseFactory.NotFound();
            }

            user.FirstName = dto.FirstName;
            user.LastName = dto.LastName;
            user.Password = dto.Password;
            user.Updated = DateTime.Now;

            var updateResult = await _userManager.UpdateAsync(user);

            return updateResult.Succeeded ? ResponseFactory.Ok() : ResponseFactory.Error();
        }
        catch (Exception ex)
        {
            _logger.LogError($"ERROR : UserService.UpdateUserAsync() :: {ex.Message}");
            return ResponseFactory.ServerError();
        }
    }

    public async Task VerificationRequest(string email)
    {
        try
        {
            var sender = _serviceBusClient.CreateSender("verification_request");
            await sender.SendMessageAsync(new ServiceBusMessage(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(new { Email = email }))));
        }
        catch (Exception)
        {

            throw;
        }
    }
}
