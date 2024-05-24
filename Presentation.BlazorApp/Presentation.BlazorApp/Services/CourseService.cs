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


    #region GetOneCourseAsync
    //public async Task<CourseOne?> GetOneCourseAsync(string courseId)
    //{
    //    try
    //    {


    //        var query = new GraphQLQuery { Query = @" ($id: ID!) { getCourseById(id: $id) { id courseTitle courseIngress courseDescription courseImageUrl isBestseller category rating { inNumbers inProcent } author { fullName biography profileImageUrl socialMedia { youTubeUrl subscribers facebookUrl followers } } prices { originalPrice discountPrice } included { hoursOfVideo articles resources lifetimeAccess certificate } } }" };

    //        var response = await _httpClient.PostAsJsonAsync("https://courseproviderv2-silicon-ev-er.azurewebsites.net/api/graphql?code=SC_MS2mU9ssVKvaSwHbS8eaAwndAzPVvGRFVe7Vq68joAzFuhzy1Dw%3D%3D", query);
    //        response.EnsureSuccessStatusCode();

    //        var result = await response.Content.ReadFromJsonAsync<DynamicGraphQLResponse>();

    //        var courseData = result?.Data.GetProperty("getCourseById");

    //        if (courseData == null) return null;

    //        return new CourseOne
    //        {
    //            Id = courseData.GetProperty("id").GetString() ?? "",
    //            CourseTitle = courseData.GetProperty("courseTitle").GetString() ?? "",
    //            CourseIngress = courseData.GetProperty("courseIngress").GetString() ?? "",
    //            CourseDescription = courseData.GetProperty("courseDescription").GetString() ?? "",
    //            CourseImageUrl = courseData.GetProperty("courseImageUrl").GetString(),
    //            IsBestseller = courseData.GetProperty("isBestseller").GetBoolean(),
    //            Category = courseData.GetProperty("category").GetString(),
    //            Rating = new Rating
    //            {
    //                InNumbers = courseData.GetProperty("rating").GetProperty("inNumbers").GetDecimal(),
    //                InProcent = courseData.GetProperty("rating").GetProperty("inProcent").GetDecimal()
    //            },
    //            Author = new Author
    //            {
    //                FullName = courseData.GetProperty("author").GetProperty("fullName").GetString() ?? "",
    //                Biography = courseData.GetProperty("author").GetProperty("biography").GetString() ?? "",
    //                ProfileImageUrl = courseData.GetProperty("author").GetProperty("profileImageUrl").GetString(),
    //                SocialMedia = new SocialMedia
    //                {
    //                    YouTubeUrl = courseData.GetProperty("author").GetProperty("socialMedia").GetProperty("youTubeUrl").GetString(),
    //                    Subscribers = courseData.GetProperty("author").GetProperty("socialMedia").GetProperty("subscribers").GetString(),
    //                    FacebookUrl = courseData.GetProperty("author").GetProperty("socialMedia").GetProperty("facebookUrl").GetString(),
    //                    Followers = courseData.GetProperty("author").GetProperty("socialMedia").GetProperty("followers").GetString()
    //                }
    //            },
    //            Highlights = courseData.GetProperty("highlights").EnumerateArray()
    //                .Select(h => new Highlights { Highlight = h.GetProperty("highlight").GetString() ?? "" })
    //                .ToList(),
    //            Content = courseData.GetProperty("content").EnumerateArray()
    //                .Select(c => new ProgramDetail
    //                {
    //                    Title = c.GetProperty("title").GetString() ?? "",
    //                    Description = c.GetProperty("description").GetString() ?? ""
    //                })
    //                .ToList(),
    //            Prices = new Price
    //            {
    //                OriginalPrice = courseData.GetProperty("prices").GetProperty("originalPrice").GetDecimal(),
    //                DiscountPrice = courseData.GetProperty("prices").GetProperty("discountPrice").GetDecimal()
    //            },
    //            Included = new Included
    //            {
    //                HoursOfVideo = courseData.GetProperty("included").GetProperty("hoursOfVideo").GetInt32(),
    //                Articles = courseData.GetProperty("included").GetProperty("articles").GetInt32(),
    //                Resources = courseData.GetProperty("included").GetProperty("resources").GetInt32(),
    //                LifetimeAccess = courseData.GetProperty("included").GetProperty("lifetimeAccess").GetBoolean(),
    //                Certificate = courseData.GetProperty("included").GetProperty("certificate").GetBoolean()
    //            }
    //        };
    //    }
    //    catch (Exception ex)
    //    {
    //        _logger.LogError(ex, "Error fetching course.");
    //        return null;
    //    }
    //}


    public async Task<CourseOne> GetOneCourseAsync(string id)
    {
        try
        {
            var query = new GraphQLQuery { Query = @" ($id: ID!) { getCourseById(id: $id) { id courseTitle courseIngress courseDescription courseImageUrl isBestseller category rating { inNumbers inProcent } author { fullName biography profileImageUrl socialMedia { youTubeUrl subscribers facebookUrl followers } } prices { originalPrice discountPrice } included { hoursOfVideo articles resources lifetimeAccess certificate } } }" };

            var response = await _httpClient.GetAsync("https://courseproviderv2-silicon-ev-er.azurewebsites.net/api/graphql?code=SC_MS2mU9ssVKvaSwHbS8eaAwndAzPVvGRFVe7Vq68joAzFuhzy1Dw%3D%3D");
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadFromJsonAsync<CourseOne>();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error fetching course: {ex.Message}");
            return null!;
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

    #region CreateCourseAsync

    public async Task<bool> CreateCourseAsync(Course course)
    {
        try
        {
            var query = new
            {
                query = "mutation ($input: CourseCreateRequestInput!) {createCourse(input: $input) {id} }",
                variables = new { input = course }
            };

            var response = await _httpClient.PostAsJsonAsync("https://courseproviderv2-silicon-ev-er.azurewebsites.net/api/graphql?code=Hdh1N9pxfiRQQL-L2i2Q9k6Kt7AZRlamhYKcr8ZVLwIAAzFuREhEZw%3D%3D", query);

            response.EnsureSuccessStatusCode();
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating course.");
            return false;
        }
    }

    #endregion


    //updateCourse modell med id input, ej datum

    //getCourseById, stringId

}
