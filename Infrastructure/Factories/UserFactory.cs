using Infrastructure.Data.Entities;
using Infrastructure.Dtos;

namespace Infrastructure.Factories;

public class UserFactory
{
    public static UserDto GetUser(ApplicationUser user)
    {
        try
        {
            return new UserDto
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email!,
                Created = user.Created,
                Updated = user.Updated,
            };
        }
        catch (Exception)
        {
            return new UserDto();
        }
    }

    public static AllUsersDto GetAllUsers(ApplicationUser user)
    {
        try
        {
            return new AllUsersDto
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email!,
            };
        }
        catch (Exception)
        {
            return new AllUsersDto();
        }
    }
}
