using Presentation.BlazorApp.Models;
using Presentation.BlazorApp.Models.Courses;
using System.Text.Json;

namespace Presentation.BlazorApp.Services;

public class CourseService
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<CourseService> _logger;

    public CourseService(HttpClient httpClient, ILogger<CourseService> logger)
    {
        _httpClient = httpClient;
        _logger = logger;
    }

    #region GetAllCoursesAsync
    public async Task<List<CourseSelect>?> GetAllCoursesAsync()
    {
        try
        {
            var query = new GraphQLQuery { Query = "{ getAllCourses { id courseTitle category } }" };

            var response = await _httpClient.PostAsJsonAsync("https://courseproviderv2-silicon-ev-er.azurewebsites.net/api/graphql?code=SC_MS2mU9ssVKvaSwHbS8eaAwndAzPVvGRFVe7Vq68joAzFuhzy1Dw%3D%3D", query);
            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadFromJsonAsync<DynamicGraphQLResponse>();

            return result?.Data.GetProperty("getAllCourses").EnumerateArray()
                .Select(course => new CourseSelect
                {
                    Id = course.GetProperty("id").GetString() ?? "",
                    CourseTitle = course.GetProperty("courseTitle").GetString() ?? "",
                    Category = course.GetProperty("category").GetString() ?? ""
                })
                .ToList();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching courses.");
            return null;
        }
    }

    #endregion

    public async Task<bool> DeleteCourseAsync(string id)
    {
        try
        {
            var query = @"mutation($id: String!) { deleteCourse(id: $id) }";
            var request = new { query, variables = new { id } };

            var response = await _httpClient.PostAsJsonAsync("https://courseproviderv2-silicon-ev-er.azurewebsites.net/api/graphql?code=SC_MS2mU9ssVKvaSwHbS8eaAwndAzPVvGRFVe7Vq68joAzFuhzy1Dw%3D%3D", request);

            response.EnsureSuccessStatusCode();
            return true; 
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting course.");
            return false;
        }
    }
}
