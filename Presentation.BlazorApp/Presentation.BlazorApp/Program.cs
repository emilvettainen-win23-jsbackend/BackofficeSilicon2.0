using Infrastructure.Data.Contexts;
using Infrastructure.Data.Entities;
using Infrastructure.Services;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Presentation.BlazorApp;
using Presentation.BlazorApp.Client.Pages;
using Presentation.BlazorApp.Components;
using Presentation.BlazorApp.Configurations;
using Presentation.BlazorApp.Hubs;
using Presentation.BlazorApp.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents()
    .AddInteractiveWebAssemblyComponents();

builder.Services.AddCascadingAuthenticationState();
builder.Services.AddScoped<AuthenticationStateProvider, PersistingRevalidatingAuthenticationStateProvider>();


builder.Services.AddScoped<HttpClient>();
builder.Services.AddScoped<CourseService>();
builder.Services.AddScoped<SubscriberService>();
builder.Services.AddScoped<UserService>();

builder.Services.AddAuthentication(options =>
    {
        options.DefaultScheme = IdentityConstants.ApplicationScheme;
        options.DefaultSignInScheme = IdentityConstants.ExternalScheme;
    })
    .AddIdentityCookies();


builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(builder.Configuration["AZURE_DB"]));


builder.Services.AddDatabaseDeveloperPageExceptionFilter();
builder.Services.AddIdentityCore<ApplicationUser>(options =>
{
    options.SignIn.RequireConfirmedAccount = true;
    options.User.RequireUniqueEmail = true;
    options.Password.RequiredLength = 8;
})
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddSignInManager()
    .AddDefaultTokenProviders();


builder.Services.AddServerSideBlazor().AddCircuitOptions(options => { options.DetailedErrors = true; });

builder.Services.AddSignalR().AddAzureSignalR(options =>
{
    options.ConnectionString = builder.Configuration["AzureSignalRConnectionString"];
});



// POLICYS
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("SuperAdminAccess", policy => policy.RequireRole("SuperAdmin"));
    options.AddPolicy("CIOAccess", policy => policy.RequireRole("SuperAdmin", "CIO"));
    options.AddPolicy("AdminAccess", policy => policy.RequireRole("SuperAdmin", "CIO", "Admin"));
    options.AddPolicy("ManagerAccess", policy => policy.RequireRole("SuperAdmin", "CIO", "Admin", "Manager"));
    options.AddPolicy("UserAccess", policy => policy.RequireRole("SuperAdmin", "CIO", "Admin", "Manager", "User"));
});





var app = builder.Build();

await RolesConfiguration.Roles(app.Services);

//ROLES
//using (var scope = app.Services.CreateScope())
//{
//    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
//    string[] roles = ["SuperAdmin", "CIO", "Admin", "Manager", "User"];
//    foreach (var role in roles)
//        if (!await roleManager.RoleExistsAsync(role))
//        {
//            await roleManager.CreateAsync(new IdentityRole(role));
//        }
//}




// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();
app.MapHub<ChatHub>("/chathub");

//added
//app.UseAuthentication();
//app.UseAuthorization();


app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode()
    .AddInteractiveWebAssemblyRenderMode()
    .AddAdditionalAssemblies(typeof(Presentation.BlazorApp.Client._Imports).Assembly);
    

// Add additional endpoints required by the Identity /Account Razor components.
app.MapAdditionalIdentityEndpoints();




// ROLES
//using (var scope = app.Services.CreateScope())
//{
//    var services = scope.ServiceProvider;
//    var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
//    await SeedRolesAsync(roleManager);
//}

//async Task SeedRolesAsync(RoleManager<IdentityRole> roleManager)
//{
//    string[] roleNames = { "SuperAdmin", "CIO", "Admin", "Manager", "User" };
//    foreach (var roleName in roleNames)
//    {
//        if (!await roleManager.RoleExistsAsync(roleName))
//        {
//            await roleManager.CreateAsync(new IdentityRole(roleName));
//        }
//    }
//}

app.Run();
