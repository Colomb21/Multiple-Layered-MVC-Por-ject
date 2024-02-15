using BilgeShop.Business.Managers;
using BilgeShop.Business.Services;
using BilgeShop.Data.Context;
using BilgeShop.Data.Repositories;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.SqlServer.Migrations.Internal;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();


var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<BilgeShopContext>(options => options.UseSqlServer(connectionString));

builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

builder.Services.AddScoped<IUserService, UserManager>();
builder.Services.AddScoped<ICategoryService, CategoryManager>();
builder.Services.AddScoped<IProductService, ProductManager>();

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(options =>
{
    options.LoginPath = new PathString("/");
    options.LogoutPath = new PathString("/");
    options.AccessDeniedPath = new PathString("/");
    // giriş - çıkış - erişim engeli durumlarında yönlendirilecek olan adresler.
});


var app = builder.Build();

app.UseStaticFiles(); // wwwroot için

app.UseAuthentication();
app.UseAuthorization();
// Auth işlemleri yapıyorsan, üstteki 2 satır yazılmalı. Yoksa hata vermez fakat oturum açamaz, yetkilendirme sorgulayamaz.


// AREA İÇİN YAZILAN ROUTE HER ZAMAN DEFAULT'UN ÜZERİNDE OLACAK

app.MapControllerRoute(
   name: "areas",
   pattern: "{area:exists}/{Controller=Dashboard}/{Action=Index}/{id?}"
    );


app.MapControllerRoute(
    name: "Default",
    pattern: "{Controller=Home}/{Action=Index}/{id?}"
    );

app.Run();





