namespace Presentation.BlazorApp.Models.Courses;

public class CourseAuthor
{
    public string FullName { get; set; } = null!;
    public string Biography { get; set; } = null!;
    public string? ProfileImageUrl { get; set; }
    public SocialMedia? SocialMedia { get; set; }
}
