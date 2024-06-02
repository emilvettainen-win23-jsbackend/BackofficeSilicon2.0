using Infrastructure.Dtos;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Infrastructure.Services;

public class CustomersService
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<CustomersService> _logger;

    public CustomersService(HttpClient httpClient, ILogger<CustomersService> logger)
    {
        _httpClient = httpClient;
        _logger = logger;
    }

    #region GetAllCustomersAsync

    public async Task <List<CustomersDto>> GetAllCustomersAsync()
    {
        try
        {
            var response = await _httpClient.GetAsync("https://userprovider-silicon-ev-er.azurewebsites.net/api/users/all?code=sIzOpzGpyKIF1T2bceoW-jKR_S8o88B-FaLQCunN4R8RAzFuDbLf-A%3D%3D");

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var customers = JsonConvert.DeserializeObject<List<CustomersDto>>(content);
                return customers ?? [];
            }
        }
        catch (Exception ex)
        {
            _logger.LogError($"ERROR: CustomersService.GetAllCustomersAsync() :: {ex.Message}");
        }
        return [];

       
       
    }
    #endregion

    #region GetOneCustomerAsync
    //READ
    #endregion

    #region UpdateCustomerAsync
    //UPDATE
    #endregion

    #region DeleteCustomerAsync
    //DELETE
    #endregion

}
