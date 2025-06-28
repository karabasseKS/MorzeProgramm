using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MorzeProgramm.Data;
using MorzeProgramm.Models;

var builder = WebApplication.CreateBuilder(args);

// Подключение строки подключения из appsettings.json
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Подключение Identity
builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders();

// Настройка куки авторизации (по желанию)
builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Account/Login";
});

// Подключение MVC
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Middleware-конвейер
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// Важно: сначала UseAuthentication, потом UseAuthorization
app.UseAuthentication();
app.UseAuthorization();

// Маршрутизация
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
