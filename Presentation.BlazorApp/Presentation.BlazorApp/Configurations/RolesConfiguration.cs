using Microsoft.AspNetCore.Identity;

namespace Presentation.BlazorApp.Configurations;

public  static class RolesConfiguration
{
    public static async Task Roles(this IServiceProvider services)
    {
        using (var scope = services.CreateScope())
        {
            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            string[] roles = [ "SuperAdmin", "CIO", "Admin", "Manager" ];
            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(new IdentityRole(role));
                }
            }
        }
    }
}
