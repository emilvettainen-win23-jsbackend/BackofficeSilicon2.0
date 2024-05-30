using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Data.Entities;

public class UserEntity : IdentityUser
{
    //public Guid Id { get; set; } = Guid.NewGuid();

    [ProtectedPersonalData]
    [Required]
    [Column(TypeName = "nvarchar(50)")]
    public string FirstName { get; set; } = null!;

    [ProtectedPersonalData]
    [Required]
    [Column(TypeName = "nvarchar(50)")]
    public string LastName { get; set; } = null!;

    //[ProtectedPersonalData]
    //public string Email { get; set; } = null!;

    public string Password { get; set; } = null!;

    [Required]
    [Column(TypeName = "datetime2")]
    public DateTime Created { get; set; }

    [Column(TypeName = "datetime2")]
    public DateTime Updated { get; set; }


}