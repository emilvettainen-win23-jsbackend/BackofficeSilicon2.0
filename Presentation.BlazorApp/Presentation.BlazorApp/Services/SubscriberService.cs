using Newtonsoft.Json;
using Presentation.BlazorApp.Models.Courses;
using Presentation.BlazorApp.Models.Subscribers;
using System.Net.Http;

namespace Presentation.BlazorApp.Services;

public class SubscriberService
{
    private readonly HttpClient _httpClient;

    public SubscriberService(HttpClient httpClient)
    {
        _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
    }

    public async Task<List<Subscribe>?> GetAllSubscribersAsync()
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
            return null;
        }
    }
}
