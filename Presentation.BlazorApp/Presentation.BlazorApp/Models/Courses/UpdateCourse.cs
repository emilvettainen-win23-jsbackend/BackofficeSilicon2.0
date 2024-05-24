namespace Presentation.BlazorApp.Models.Courses;

public class UpdateCourse
{
    public string Id { get; set; } = null!;
    public string CourseTitle { get; set; } = null!;
    public string CourseIngress { get; set; } = null!;
    public string CourseDescription { get; set; } = null!;
    public string? CourseImageUrl { get; set; }
    public bool IsBestseller { get; set; }
    public string? Category { get; set; }
    public Rating Rating { get; set; } = new Rating();
    public Author Author { get; set; } = new Author();
    public List<Highlights> Highlights { get; set; } = new List<Highlights>();
    public List<ProgramDetail> Content { get; set; } = new List<ProgramDetail>();
    public Price Prices { get; set; } = new Price();
    public Included Included { get; set; } = new Included();
}


