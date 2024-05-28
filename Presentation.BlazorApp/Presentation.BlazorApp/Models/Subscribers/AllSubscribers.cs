using System.ComponentModel.DataAnnotations;

namespace Presentation.BlazorApp.Models.Subscribers;

public class AllSubscribers
{

    [Required]
    public string Id { get; set; } = null!;

    [Required]
    [EmailAddress]
    [RegularExpression(@"^(([^<>()\]\\.,;:\s@\""]+(\.[^<>()\]\\.,;:\s@\""]+)*)|("".+""))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$")]
    public string Email { get; set; } = null!;
}
