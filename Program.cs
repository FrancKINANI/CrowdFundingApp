using CrowdFundingApp.Models;
using CrowdFundingApp.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

//-------------------------
// Service Configuration
//-------------------------

// Configure Entity Framework with SQL Server
builder.Services.AddDbContext<CrowdFundingDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("CrowdFundingConnection")));

// Configure Identity for user and role management
builder.Services.AddIdentity<User, IdentityRole>(options =>
{
    options.Password.RequireDigit = true;
    options.Password.RequiredLength = 6;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
})
    .AddEntityFrameworkStores<CrowdFundingDbContext>()
    .AddDefaultTokenProviders();

// Configure cookie settings for authentication
builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Account/Login";
    options.LogoutPath = "/Account/Logout";
    options.AccessDeniedPath = "/Account/AccessDenied";
    options.ExpireTimeSpan = TimeSpan.FromHours(2);
    options.SlidingExpiration = true;
    options.Cookie.HttpOnly = true;
    options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
});

// Add controllers and views
builder.Services.AddControllersWithViews();

// Add logging and custom services
builder.Services.AddLogging();
builder.Services.AddScoped<IFileService, FileService>();

var app = builder.Build();

//-------------------------
// Middleware Configuration
//-------------------------

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

// Define default routing pattern
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

//-------------------------
// Role and Admin Initialization
//-------------------------

async Task InitializeRoles(IServiceProvider serviceProvider)
{
    var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
    var userManager = serviceProvider.GetRequiredService<UserManager<User>>();

    // Roles to be created
    var roles = new[] { "Admin", "Startup", "Investor", "User" };

    foreach (var role in roles)
    {
        if (!await roleManager.RoleExistsAsync(role))
        {
            await roleManager.CreateAsync(new IdentityRole(role));
        }
    }

    // Create a default admin user
    var adminEmail = "admin@crowdfunding.com";
    var adminUser = await userManager.FindByEmailAsync(adminEmail);
    if (adminUser == null)
    {
        var admin = new User
        {
            UserName = "admin",
            Email = adminEmail,
            EmailConfirmed = true
        };

        var result = await userManager.CreateAsync(admin, "Admin1234!");
        if (result.Succeeded)
        {
            await userManager.AddToRoleAsync(admin, "Admin");
        }
    }
}

// Call the role initialization method
using (var scope = app.Services.CreateScope())
{
    var serviceProvider = scope.ServiceProvider;
    await InitializeRoles(serviceProvider);
}

//-------------------------
// Application Execution
//-------------------------
app.Run();
