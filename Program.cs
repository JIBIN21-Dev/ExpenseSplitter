using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ExpenseSplitter.Data;
using ExpenseSplitter.Models;

var builder = WebApplication.CreateBuilder(args);

// --------------------
// SERVICES
// --------------------
builder.Services.AddControllersWithViews();

// --------------------
// DATABASE
// --------------------
builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    var connectionString = Environment.GetEnvironmentVariable("DATABASE_URL");

    if (!string.IsNullOrEmpty(connectionString))
    {
        // ✅ POSTGRES (Render)
        var uri = new Uri(connectionString);
        var userInfo = uri.UserInfo.Split(':');

        options.UseNpgsql(
            $"Host={uri.Host};Port={uri.Port};Database={uri.AbsolutePath.TrimStart('/')};" +
            $"Username={userInfo[0]};Password={userInfo[1]};" +
            $"SSL Mode=Require;Trust Server Certificate=true"
        );
    }
    else
    {
        // ✅ SQLITE (Local)
        options.UseSqlite("Data Source=expensesplitter.db");
    }
});

// --------------------
// IDENTITY
// --------------------
builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
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
