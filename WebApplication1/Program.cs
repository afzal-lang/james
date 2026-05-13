using Microsoft.EntityFrameworkCore;
using WebApplication1.Models;

var builder = WebApplication.CreateBuilder(args);

// ======================
// MVC
// ======================
builder.Services.AddControllersWithViews();

// ======================
// DATABASE
// ======================
builder.Services.AddDbContext<JamesthewContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")
    )
);

// ======================
// HTTP CONTEXT
// ======================
builder.Services.AddHttpContextAccessor();

// ======================
// SESSION
// ======================
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

// ======================
// AUTHENTICATION (COOKIE)
// ======================
builder.Services.AddAuthentication("MyCookieAuth")
    .AddCookie("MyCookieAuth", options =>
    {
        options.LoginPath = "/cookies/Login";
        options.LogoutPath = "/cookies/Logout";
        options.Cookie.Name = "MyCookieAuth";
        options.ExpireTimeSpan = TimeSpan.FromHours(2);
        options.SlidingExpiration = true;
        options.AccessDeniedPath = "/cookies/Login";
    });

var app = builder.Build();

// ======================
// MIDDLEWARE PIPELINE
// ======================
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseSession();
app.UseAuthentication();
app.UseAuthorization();

// ======================
// ROUTES
// ======================


// DEFAULT ROUTE
app.MapControllerRoute(
    name: "default",
   pattern: "{controller=user}/{action=Index}/{id?}"
);

// FEEDBACK ROUTE
app.MapControllerRoute(
    name: "feedback",
   pattern: "Feedback/{action=Create}/{id?}"
);
app.MapControllerRoute(
    name: "contest",
    pattern: "Contest/{action=Index}/{id?}",
    defaults: new { controller = "Contest" }
);

app.Run();