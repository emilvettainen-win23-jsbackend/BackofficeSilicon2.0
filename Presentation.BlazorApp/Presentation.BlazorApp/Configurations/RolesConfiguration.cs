﻿using Microsoft.AspNetCore.Identity;

namespace Presentation.BlazorApp.Configurations;

public  static class RolesConfiguration
{
    //public static async Task Roles(IServiceProvider services)
    //{
    //    using (var scope = services.CreateScope())
    //    {
    //        var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

    //        string[] roleNames = { "SuperAdmin", "CIO", "Admin", "Manager", "User" };

    //        foreach (var roleName in roleNames)
    //        {
    //            if (!await roleManager.RoleExistsAsync(roleName))
    //            {
    //                await roleManager.CreateAsync(new IdentityRole(roleName));
    //            }
    //        }
    //    }
    //}

    public static async Task Roles(this IServiceProvider services)
    {
        using (var scope = services.CreateScope())
        {
            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            string[] roles = ["SuperAdmin", "CIO", "Admin", "Manager" ];
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