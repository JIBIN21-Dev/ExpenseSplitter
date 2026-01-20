using ExpenseSplitter.Data;
using ExpenseSplitter.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// --------------------
// SERVICES
// --------------------
builder.Services.AddControllersWithViews();

// --------------------
// DATABASE CONFIG
// --------------------
builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    var databaseUrl = Environment.GetEnvironmentVariable("DATABASE_URL");

    if (!string.IsNullOrEmpty(databaseUrl))
    {
        // ✅ POSTGRES (Render)
        var uri = new Uri(databaseUrl);
        var userInfo = uri.UserInfo.Split(':');

        var host = uri.Host;
        var database = uri.AbsolutePath.TrimStart('/');
        var username = userInfo[0];
        var password = userInfo[1];
        var port = uri.Port > 0 ? uri.Port : 5432; // ✅ FIXED

        var conn =
            $"Host={host};Port={port};Database={database};" +
            $"Username={username};Password={password};" +
            $"SSL Mode=Require;Trust Server Certificate=true";

        options.UseNpgsql(conn);
    }
    else
    {
        // ✅ LOCAL SQLITE
        options.UseSqlite("Data Source=expensesplitter.db");
    }
});

// --------------------
// IDENTITY
// --------------------
builder.Services
    .AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Account/Login";
});

// --------------------
// BUILD
// --------------------
var app = builder.Build();

// --------------------
// AUTO MIGRATION (SAFE)
// --------------------
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    db.Database.Migrate();
}

// --------------------
// MIDDLEWARE
// --------------------
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
