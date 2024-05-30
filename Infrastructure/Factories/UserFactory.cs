using Infrastructure.Data.Entities;
using Infrastructure.Dtos;
using System.Diagnostics;

namespace Infrastructure.Factories;

public class UserFactory
{
    //    public static UserEntity CreateUser(UserRegistrationForm form)
    //    {
    //        try
    //        {
    //            return new UserEntity
    //            {
    //                Id = Guid.NewGuid().ToString(),
    //                FirstName = form.FirstName,
    //                LastName = form.LastName,   
    //                Email = form.Email, 
    //                Password = PasswordHasher.GenerateSecurePassword(form.Password),
    //            };
    //        }
    //        catch { }
    //        return null!;
    //    }
    public static UserDto GetUser(UserEntity user)
    {
        try
        {
            return new UserDto
            {
                Id = Guid.NewGuid().ToString(),
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email!,
                Password = user.Password,
                Created = user.Created,
                Updated = user.Updated,
            };
        }
        catch (Exception)
        {

            return new UserDto();
        }
    }
}
