using DataLayer.Context;
using LinkedNewsChatApp.Data;
using LinkedNewsChatApp.Hubs;
using LinkedNewsChatApp.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddSignalR();

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

builder.Services.AddDbContext<LinkedNewsDbContext>();
//2 дб контекст
var connectionString = builder.Configuration.GetConnectionString("LinkedNewsDb");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddScoped<LinkedNewsChatApp.Data.NewsRepository>();
builder.Services.AddScoped<DataLayer.LinkedNewsRepository>();
builder.Services.AddScoped<ServiceLayer.LoginOperations>();
builder.Services.AddScoped<ServiceLayer.ChatOperations>();
builder.Services.AddScoped<ServiceLayer.MailService>();
builder.Services.AddScoped<UserManagerRepository>();
builder.Services.AddHttpContextAccessor();
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Home/Index";
        options.Cookie.Name = "LoginCookie";
        options.ExpireTimeSpan = TimeSpan.FromHours(24);
    });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.UseCookiePolicy();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapHub<ChatHub>("/chatHub");

app.Run();