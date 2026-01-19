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
        options.UseNpgsql(
            builder.Configuration.GetConnectionString("DefaultConnection")
        );
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
// MIDDLEWARE
// ----------------------------------
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

// ❌ DISABLED FOR NOW
// app.UseHttpsRedirection();

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

// ----------------------------------
// AUTO MIGRATION
// ----------------------------------
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    context.Database.Migrate();
}

Console.WriteLine("Application started successfully!");
app.Run();
