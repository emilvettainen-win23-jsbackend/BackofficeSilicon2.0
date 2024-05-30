using Infrastructure.Data.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data.Contexts;

public class UserDbContext(DbContextOptions<UserDbContext> options) : IdentityDbContext<UserEntity>(options)
{

}