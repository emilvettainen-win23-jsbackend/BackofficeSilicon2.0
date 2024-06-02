using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Infrastructure.Data.Entities;

public class ApplicationUser : IdentityUser
{
    [ProtectedPersonalData]
    [Required]
    [Column(TypeName = "nvarchar(50)")]
    public string FirstName { get; set; } = null!;

    [ProtectedPersonalData]
    [Required]
    [Column(TypeName = "nvarchar(50)")]
    public string LastName { get; set; } = null!;

    [Required]
    [Column(TypeName = "datetime2")]
    public DateTime Created { get; set; }

    [Column(TypeName = "datetime2")]
    public DateTime Updated { get; set; }
}
