using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MorzeProgramm.Data;
using MorzeProgramm.Models;

var builder = WebApplication.CreateBuilder(args);

// ����������� ������ ����������� �� appsettings.json
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// ����������� Identity
builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders();

// ��������� ���� ����������� (�� �������)
builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Account/Login";
});

// ����������� MVC
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Middleware-��������
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// �����: ������� UseAuthentication, ����� UseAuthorization
app.UseAuthentication();
app.UseAuthorization();

// �������������
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
