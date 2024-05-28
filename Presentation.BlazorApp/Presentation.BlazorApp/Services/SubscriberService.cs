using Microsoft.EntityFrameworkCore.Query.Internal;
using Newtonsoft.Json;
using Presentation.BlazorApp.Models.Courses;
using Presentation.BlazorApp.Models.Subscribers;
using System.Net.Http;
using System.Text;

namespace Presentation.BlazorApp.Services;

public class SubscriberService
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<SubscriberService> _logger;

    public SubscriberService(HttpClient httpClient, ILogger<SubscriberService> logger)
    {
        _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        _logger = logger;
    }

    public async Task<List<Subscribe>?> GetAllSubscribersAsync()
    {
        try
        {
            var response = await _httpClient.GetAsync("https://subscribeprovider-silicon-ev-er.azurewebsites.net/api/subscribers?code=X0oaGflNTJJ2tC1tWhMHTIAXQZrhzL_B9mj-d5gyS8S5AzFucWUhLQ%3D%3D");

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var subscribers = JsonConvert.DeserializeObject<List<Subscribe>>(content);
                return subscribers;
            }
            else
            {
                return new List<Subscribe>();
            }
        }
        catch (Exception ex) 
        {
            _logger.LogError(ex, "Error fetching subscribers.");
            return new List<Subscribe>();
        }

    }

    public async Task<bool> DeleteSubscriberAsync(string email)
    {
        try
        {
            var payload = new StringContent($"{{ \"email\": \"{email}\" }}", Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync("https://subscribeprovider-silicon-ev-er.azurewebsites.net/api/delete?code=H9rAxAUCBEYTDONjEl5gTJ8NyGBdxXxMS8WrHlGu214EAzFuFqXTAA%3D%3D", payload);

            if (response.IsSuccessStatusCode)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting subscriber.");
            return false;
        }
    }




}

//getone (id)
//https://subscribeprovider-silicon-ev-er.azurewebsites.net/api/subscriber/{id}?code=GA5ZwspjZrIvu2RyVW2nYhmLQwt8PDZRS3f8otWRA_jIAzFuHNtZTA%3D%3D


//delete (email)
//https://subscribeprovider-silicon-ev-er.azurewebsites.net/api/delete?code=H9rAxAUCBEYTDONjEl5gTJ8NyGBdxXxMS8WrHlGu214EAzFuFqXTAA%3D%3D

//update (allt, id och email required)
//https://subscribeprovider-silicon-ev-er.azurewebsites.net/api/UpdateSubscription?code=YNfeYvnfofwXXOuU1EfS82nf_B1v1ICHDPXHeGFvdhjGAzFuvtJsQw%3D%3D