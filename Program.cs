using CrowdFundingApp.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Adding services for controllers and views
builder.Services.AddDbContext<CrowdFundingDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("CrowdFundingConnection")));

builder.Services.Configure<CookiePolicyOptions>(options =>
{
    options.MinimumSameSitePolicy = SameSiteMode.Unspecified;
    options.Secure = CookieSecurePolicy.None;
});

// Setting up cookie authentication
builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Account/Login";
    options.LogoutPath = "/Account/Logout"; 
    options.AccessDeniedPath = "/Account/AccessDenied";
});

// Configuration of the identity
builder.Services.AddIdentity<User, IdentityRole>(options =>
{
    options.Password.RequireDigit = true;
    options.Password.RequiredLength = 6;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
})
    .AddEntityFrameworkStores<CrowdFundingDbContext>()
    .AddDefaultTokenProviders();


builder.Services.ConfigureApplicationCookie(options =>
{
    options.ExpireTimeSpan = TimeSpan.FromMinutes(30);
    options.SlidingExpiration = false;
    options.Cookie.HttpOnly = true;
    options.Cookie.Expiration = null;
});

builder.Services.AddControllersWithViews();

var app = builder.Build();

// Error management and security
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

// Middleware for routing, security, authentication and static files
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

// Definition of routes
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

// Call for role initialization
using (var scope = app.Services.CreateScope())
{
    var serviceProvider = scope.ServiceProvider;
    await InitializeRoles(serviceProvider);
}

// execution of the application
app.Run();

// Method to initialize roles and administrator user
async Task InitializeRoles(IServiceProvider serviceProvider)
{
    var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
    var userManager = serviceProvider.GetRequiredService<UserManager<User>>();

    // Check and create the Admin role
    if (!await roleManager.RoleExistsAsync("Admin"))
    {
        await roleManager.CreateAsync(new IdentityRole("Admin"));
    }

    // Check and create the User role
    if (!await roleManager.RoleExistsAsync("User"))
    {
        await roleManager.CreateAsync(new IdentityRole("User"));
    }

    // Create a default administrator user if it does not exist
    var adminUser = await userManager.FindByEmailAsync("admin@crowdfunding.com");
    if (adminUser == null)
    {
        var admin = new User
        {
            UserName = "admin",
            Email = "admin@crowdfunding.com",
            EmailConfirmed = true
        };
        var result = await userManager.CreateAsync(admin, "Admin1234!");
        if (result.Succeeded)
        {
            await userManager.AddToRoleAsync(admin, "Admin");
        }
    }
}