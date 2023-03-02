using CI_Platform.Models.Models;
using CI_Platform.Repository.Interface;
using CI_Platform.Repository.Repositories;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<CiPlatformContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<IRepository<User>, Repository<User>>();
builder.Services.AddScoped<ILostPasswordRepository, LostPasswordRepository>();
builder.Services.AddScoped<IResetPasswordRepository, ResetPasswordRepository>();


builder.Services.AddScoped<IRegistrationRepository, RegistrationRepository>();
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

app.UseAuthorization();

app.MapControllerRoute(
    name: "Default",
    pattern: "{controller=Auth}/{action=Login}");

app.MapControllerRoute(
    name: "Auth",
    pattern: "{controller=Auth}/{action=Registration}");

app.MapControllerRoute(
    name: "Auth",
    pattern: "{controller=Auth}/{action=Lost_Password}");

app.MapControllerRoute(
    name: "Auth",
    pattern: "{controller=Auth}/{action=Reset_Password}");

app.MapControllerRoute(
    name: "Home",
    pattern: "{controller=Home}/{action=MissionListing}");

app.MapControllerRoute(
    name: "Home",
    pattern: "{controller=Home}/{action=VolunteeringMission}");

app.MapControllerRoute(
    name: "Home",
    pattern: "{controller=Home}/{action=StoryListing}");

app.Run();
