using System.Text.Json;

namespace Presentation.BlazorApp.Models;

public class DynamicGraphQLResponse
{
    public JsonElement Data { get; set; }
}
