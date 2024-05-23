using Microsoft.EntityFrameworkCore.Storage.Json;
using Newtonsoft.Json;
using Presentation.BlazorApp.Models;
using Presentation.BlazorApp.Models.Courses;
using System.Text;
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

    #region GetAllAuthorsAsync
    public async Task<List<CourseAuthor>?> GetAllAuthorsAsync()
    {
        try
        {
            var query = new GraphQLQuery { Query = "{ getAllCourses { author { fullName } }" };

            var response = await _httpClient.PostAsJsonAsync("https://courseproviderv2-silicon-ev-er.azurewebsites.net/api/graphql?code=SC_MS2mU9ssVKvaSwHbS8eaAwndAzPVvGRFVe7Vq68joAzFuhzy1Dw%3D%3D", query);
            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadFromJsonAsync<DynamicGraphQLResponse>();

            return result?.Data.GetProperty("getAllCourses").EnumerateArray()
                .Select(course => new CourseAuthor
                {
                    FullName = course.GetProperty("fullName").GetString() ?? ""
                })
                .ToList();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching authors.");
            return null;
        }
    }

    #endregion

    #region GetAllCategoriesAsync
    public async Task<List<CourseCategory>?> GetAllCategoriesAsync()
    {
        try
        {
            var query = new GraphQLQuery { Query = "{ getAllCategories }" };

            var response = await _httpClient.PostAsJsonAsync("https://courseproviderv2-silicon-ev-er.azurewebsites.net/api/graphql?code=SC_MS2mU9ssVKvaSwHbS8eaAwndAzPVvGRFVe7Vq68joAzFuhzy1Dw%3D%3D", query);
            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadFromJsonAsync<DynamicGraphQLResponse>();

            return result?.Data.GetProperty("getAllCourses").EnumerateArray()
                .Select(course => new CourseCategory
                {
                    Category = course.GetProperty("category").GetString() ?? ""
                })
                .ToList();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching authors.");
            return null;
        }
    }

    #endregion

    #region DeleteCourseAsync
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

    #endregion

    public async Task<bool> CreateCourseAsync(Course course)
    {
        try
        {
            var query = new
            {
                query = "mutation ($input: CourseCreateRequestInput!) {createCourse(input: $input {id} }",
                variables = new { course }
            };

            //var jsonQuery = JsonConvert.SerializeObject(query);
            //var content = new StringContent(jsonQuery, Encoding.UTF8, "application/json");

            //var response = await _httpClient.PostAsync("", content);

           



            var response = await _httpClient.PostAsJsonAsync("https://courseproviderv2-silicon-ev-er.azurewebsites.net/api/graphql?code=SC_MS2mU9ssVKvaSwHbS8eaAwndAzPVvGRFVe7Vq68joAzFuhzy1Dw%3D%3D", query);

            response.EnsureSuccessStatusCode();
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating course.");
            return false;
        }

    }

}
