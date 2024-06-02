namespace Infrastructure.Dtos;

public class UserDto
{
    public string Id { get; set; } = null!;
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string Password { get; set; } = null!;
    public string ConfirmPassword { get; set;} = null!;
    public string Role { get; set; } = null!;
    public DateTime Created { get; set; } = DateTime.Now;
    public DateTime Updated { get; set; } = new DateTime();

}
