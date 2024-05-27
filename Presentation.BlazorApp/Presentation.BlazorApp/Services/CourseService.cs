using Azure.Core;
using Microsoft.EntityFrameworkCore.Storage.Json;
using Newtonsoft.Json;
using Presentation.BlazorApp.Models;
using Presentation.BlazorApp.Models.Courses;
using System.Text;
using System.Text.Json;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

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

    public async Task<CourseOne> GetOneCourseAsync(string id)
    {
        try
        {
            var query = @" query ($id: String!) { getCourseById(id: $id) { id courseTitle courseIngress courseDescription courseImageUrl isBestseller category created lastUpdated rating { inNumbers inProcent } prices {originalPrice discountPrice } included { hoursOfVideo articles resources lifetimeAccess certificate } author { fullName biography profileImageUrl socialMedia { youTubeUrl subscribers facebookUrl followers } } highlights { highlight } content { title description } } }";
            var request = new { query, variables = new { id } };

            var response = await _httpClient.PostAsJsonAsync("https://courseproviderv2-silicon-ev-er.azurewebsites.net/api/graphql?code=SC_MS2mU9ssVKvaSwHbS8eaAwndAzPVvGRFVe7Vq68joAzFuhzy1Dw%3D%3D", request);
            response.EnsureSuccessStatusCode();

            var responseContent = await response.Content.ReadAsStringAsync();
            var document = JsonDocument.Parse(responseContent);
            var root = document.RootElement;

            if (root.GetProperty("data").TryGetProperty("getCourseById", out var courseElement))
            {
                var course = new CourseOne
                {
                    Id = courseElement.GetProperty("id").GetString() ?? "",
                    CourseTitle = courseElement.GetProperty("courseTitle").GetString() ?? "",
                    CourseIngress = courseElement.GetProperty("courseIngress").GetString() ?? "",
                    CourseDescription = courseElement.GetProperty("courseDescription").GetString() ?? "",
                    CourseImageUrl = courseElement.GetProperty("courseImageUrl").GetString() ?? "",
                    IsBestseller = courseElement.GetProperty("isBestseller").GetBoolean(),
                    Category = courseElement.GetProperty("category").GetString() ?? "",
                    //Created = courseElement.GetProperty("created").GetDateTime(),
                    //LastUpdated = courseElement.GetProperty("lastUpdated").GetDateTime(),
                    Rating = courseElement.TryGetProperty("rating", out var ratingElement) ? new Rating
                    {
                        InNumbers = ratingElement.GetProperty("inNumbers").GetDecimal(),
                        InProcent = ratingElement.GetProperty("inProcent").GetDecimal()
                    } /*: new Rating(),*/ : null!,
                    Author = courseElement.TryGetProperty("author", out var authorElement) ? new Author
                    {
                        FullName = authorElement.GetProperty("fullName").GetString() ?? "",
                        Biography = authorElement.GetProperty("biography").GetString() ?? "",
                        ProfileImageUrl = authorElement.GetProperty("profileImageUrl").GetString() ?? "",
                        SocialMedia = authorElement.TryGetProperty("socialMedia", out var socialMediaElement) ? new SocialMedia
                        {
                            YouTubeUrl = socialMediaElement.GetProperty("youTubeUrl").GetString() ?? "",
                            Subscribers = socialMediaElement.GetProperty("subscribers").GetString() ?? "",
                            FacebookUrl = socialMediaElement.GetProperty("facebookUrl").GetString() ?? "",
                            Followers = socialMediaElement.GetProperty("followers").GetString() ?? ""
                        } /*: new SocialMedia()*/ : null
                    } /*: new Author(),*/ : null!,
                    Highlights = courseElement.TryGetProperty("highlights", out var highlightsElement) ? highlightsElement.EnumerateArray().Select(highlightElement => new Highlights
                    {
                        Highlight = highlightElement.GetProperty("highlight").GetString() ?? ""
                    }).ToList() : new List<Highlights>(),
                    Content = courseElement.TryGetProperty("content", out var contentElement) ? contentElement.EnumerateArray().Select(contentElement => new ProgramDetail
                    {
                        Title = contentElement.GetProperty("title").GetString() ?? "",
                        Description = contentElement.GetProperty("description").GetString() ?? ""
                    }).ToList() : new List<ProgramDetail>(),
                    Prices = courseElement.TryGetProperty("prices", out var priceElement) ? new Price
                    {
                        OriginalPrice = priceElement.GetProperty("originalPrice").GetDecimal(),
                        DiscountPrice = priceElement.GetProperty("discountPrice").GetDecimal()
                    } /*: new Price(),*/ : null!,
                    Included = courseElement.TryGetProperty("included", out var includedElement) ? new Included
                    {
                        HoursOfVideo = includedElement.GetProperty("hoursOfVideo").GetInt32(),
                        Articles = includedElement.GetProperty("articles").GetInt32(),
                        Resources = includedElement.GetProperty("resources").GetInt32(),
                        LifetimeAccess = includedElement.GetProperty("lifetimeAccess").GetBoolean(),
                        Certificate = includedElement.GetProperty("certificate").GetBoolean()
                    } /*: new Included()*/ : null!,
                };

                return course;
            }

            return new CourseOne();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error fetching course: {ex.Message}");
            return new CourseOne(); 
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

    #region UpdateCourseAsync

    public async Task<bool> UpdateCourseAsync(CourseOne course)
    {
        try
        {
            var query = @"mutation updateCourse($input: CourseUpdateRequestInput!) { updateCourse (input: $input) { id } }";

            var request = new
            {
                query,
                variables = new { input = course }
            };

            var response = await _httpClient.PostAsJsonAsync("https://courseproviderv2-silicon-ev-er.azurewebsites.net/api/graphql?code=Hdh1N9pxfiRQQL-L2i2Q9k6Kt7AZRlamhYKcr8ZVLwIAAzFuREhEZw%3D%3D", request);

            response.EnsureSuccessStatusCode();
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating course.");
            return false;
        }

    }

    #endregion


}
