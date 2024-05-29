using Newtonsoft.Json;
using Presentation.BlazorApp.Models.Subscribers;
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

    #region GetAllSubscribersAsync
    public async Task<List<AllSubscribers>?> GetAllSubscribersAsync()
    {
        try
        {
            var response = await _httpClient.GetAsync("https://subscribeprovider-silicon-ev-er.azurewebsites.net/api/subscribers?code=X0oaGflNTJJ2tC1tWhMHTIAXQZrhzL_B9mj-d5gyS8S5AzFucWUhLQ%3D%3D");

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var subscribers = JsonConvert.DeserializeObject<List<AllSubscribers>>(content);
                return subscribers;
            }
            else
            {
                return new List<AllSubscribers>();
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching subscribers.");
            return new List<AllSubscribers>();
        }
    }

    #endregion


    #region DeleteSubscriberAsync
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

    #endregion


    #region GetOneSubscriberAsync
    public async Task<OneSubscriber> GetOneSubscriberAsync(string id)
    {
        try
        {

            var response = await _httpClient.GetAsync($"https://subscribeprovider-silicon-ev-er.azurewebsites.net/api/subscriber/{id}?code=GA5ZwspjZrIvu2RyVW2nYhmLQwt8PDZRS3f8otWRA_jIAzFuHNtZTA%3D%3D");

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var subscriber = JsonConvert.DeserializeObject<OneSubscriber>(content);
                return subscriber!;
            }
            else
            {
                return new OneSubscriber();
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching subscriber.");
            return new OneSubscriber();
        }
    }

    #endregion


    #region UpdateSubscriptionAsync

    public async Task<bool> UpdateSubscriptionAsync(OneSubscriber updatedSubscriber)
    {
        try
        {
            var json = JsonConvert.SerializeObject(updatedSubscriber);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync("https://subscribeprovider-silicon-ev-er.azurewebsites.net/api/UpdateSubscription?code=YNfeYvnfofwXXOuU1EfS82nf_B1v1ICHDPXHeGFvdhjGAzFuvtJsQw%3D%3D", content);

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
            _logger.LogError(ex, "Error updating subscriber.");
            return false;
        }
    }

    #endregion


}
