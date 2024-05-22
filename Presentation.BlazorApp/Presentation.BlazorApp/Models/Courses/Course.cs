namespace Presentation.BlazorApp.Models.Courses
{
    public class Course
    {
        public string Id { get; set; } = null!;
        public string CourseTitle { get; set; } = null!;
        public string CourseIngress { get; set; } = null!;
        public string CourseDescription { get; set; } = null!;
        public string? CourseImageUrl { get; set; }
        public bool IsBestseller { get; set; }
        public string? Category { get; set; }
        public Rating Rating { get; set; } = null!;
        public Author Author { get; set; } = null!;
        public List<Highlights> Highlights { get; set; } = null!;
        public List<ProgramDetail> Content { get; set; } = null!;
        public Price Prices { get; set; } = null!;
        public Included Included { get; set; } = null!;
        public DateTime Created { get; set; }
        public DateTime LastUpdated { get; set; }
    }

    public class Rating
    {
        public decimal InNumbers { get; set; }
        public decimal InProcent { get; set; }
    }

    public class Author
    {
        public string FullName { get; set; } = null!;
        public string Biography { get; set; } = null!;
        public string? ProfileImageUrl { get; set; }
        public SocialMedia? SocialMedia { get; set; }
    }
    public class Highlights
    {
        public string Highlight { get; set; } = null!;
    }

    public class SocialMedia
    {
        public string? YouTubeUrl { get; set; }
        public string? Subscribers { get; set; }
        public string? FacebookUrl { get; set; }
        public string? Followers { get; set; }
    }

    public class ProgramDetail
    {
        public string Title { get; set; } = null!;
        public string Description { get; set; } = null!;
    }

    public class Price
    {
        public decimal OriginalPrice { get; set; }
        public decimal? DiscountPrice { get; set; }
    }

    public class Included
    {
        public int HoursOfVideo { get; set; }
        public int Articles { get; set; }
        public int Resources { get; set; }
        public bool LifetimeAccess { get; set; }
        public bool Certificate { get; set; }
    }
}
