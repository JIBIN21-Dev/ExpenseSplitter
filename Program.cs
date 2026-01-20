using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ExpenseSplitter.Data;
using ExpenseSplitter.Models;

var builder = WebApplication.CreateBuilder(args);

// ----------------------------------
// ADD SERVICES
// ----------------------------------
builder.Services.AddControllersWithViews();

// ----------------------------------
// DATABASE CONFIGURATION
// ----------------------------------
builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    if (builder.Environment.IsDevelopment())
    {
        // LOCAL SQLITE
        options.UseSqlite("Data Source=expensesplitter.db");
    }
    else
    {
        // PRODUCTION - POSTGRES (Render)
        var connectionString = Environment.GetEnvironmentVariable("DATABASE_URL");

        if (!string.IsNullOrEmpty(connectionString))
        {
            // Parse Render PostgreSQL URL format
            var uri = new Uri(connectionString);
            var host = uri.Host;
            var database = uri.AbsolutePath.TrimStart('/');
            var userInfo = uri.UserInfo.Split(':');
            var username = userInfo[0];
            var password = userInfo[1];

            // Use default PostgreSQL port if not specified
            var port = uri.Port > 0 ? uri.Port : 5432;

            var npgsqlConnectionString =
                $"Host={host};Port={port};Database={database};Username={username};Password={password};SSL Mode=Require;Trust Server Certificate=true";

            Console.WriteLine($"📊 Connecting to database: {host}:{port}/{database}");
            options.UseNpgsql(npgsqlConnectionString);
        }
        else
        {
            // Fallback to appsettings
            options.UseNpgsql(
                builder.Configuration.GetConnectionString("DefaultConnection")
            );
        }
    }
});

// ----------------------------------
// IDENTITY CONFIGURATION
// ----------------------------------
builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
{
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireUppercase = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequiredLength = 6;
    options.User.RequireUniqueEmail = true;
    options.SignIn.RequireConfirmedEmail = false;
})
.AddEntityFrameworkStores<ApplicationDbContext>()
.AddDefaultTokenProviders();

// ----------------------------------
// COOKIE CONFIG
// ----------------------------------
builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Account/Login";
    options.LogoutPath = "/Account/Logout";
    options.AccessDeniedPath = "/Account/AccessDenied";
});

// ----------------------------------
// SESSION
// ----------------------------------
builder.Services.AddSession();

var app = builder.Build();

// ----------------------------------
// AUTO MIGRATION (RUN BEFORE MIDDLEWARE)
// ----------------------------------
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<ApplicationDbContext>();
        var logger = services.GetRequiredService<ILogger<Program>>();

        logger.LogInformation("🔄 Starting database migration...");
        context.Database.Migrate();
        logger.LogInformation("✅ Database migration completed successfully!");
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "❌ An error occurred while migrating the database.");
        throw; // Re-throw to prevent app from starting with bad DB
    }
}

// ----------------------------------
// MIDDLEWARE
// ----------------------------------
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.UseSession();

// ----------------------------------
// ROUTING
// ----------------------------------
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

Console.WriteLine("🚀 Application started successfully!");
app.Run();