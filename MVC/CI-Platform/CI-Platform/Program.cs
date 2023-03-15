using CI_Platform.Models.Models;
using CI_Platform.Repository.Interface;
using CI_Platform.Repository.Repositories;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<CiPlatformContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<IRepository<User>, Repository<User>>();
builder.Services.AddScoped<IRepository<City>, Repository<City>>();
builder.Services.AddScoped<IRepository<Country>, Repository<Country>>();
builder.Services.AddScoped<IRepository<MissionTheme>, Repository<MissionTheme>>();
builder.Services.AddScoped<IRepository<Skill>, Repository<Skill>>();
builder.Services.AddScoped<IRepository<Mission>, Repository<Mission>>();
builder.Services.AddScoped<IRepository<MissionMedium>, Repository<MissionMedium>>();
builder.Services.AddScoped<IRepository<MissionRating>, Repository<MissionRating>>();
builder.Services.AddScoped<IRepository<GoalMission>, Repository<GoalMission>>();
builder.Services.AddScoped<IRepository<MissionSeat>, Repository<MissionSeat>>();
builder.Services.AddScoped<IRepository<MissionSkill>, Repository<MissionSkill>>();
builder.Services.AddScoped<IRepository<FavoriteMission>, Repository<FavoriteMission>>();

builder.Services.AddScoped<ILostPasswordRepository, LostPasswordRepository>();
builder.Services.AddScoped<ILoginRepository, LoginRepository>();
builder.Services.AddScoped<IResetPasswordRepository, ResetPasswordRepository>();
builder.Services.AddScoped<IRegistrationRepository, RegistrationRepository>();

builder.Services.AddScoped<IMissionListingRepository, MissionListingRepository>();
builder.Services.AddScoped<IMissionCardRepository,MissionCardRepository>();



builder.Services.AddSession();
builder.Services.AddMemoryCache();
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
app.UseSession();
app.MapControllerRoute(
    name: "Default",
    pattern: "{controller=Auth}/{action=Login}");

/*app.MapControllerRoute(
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
    pattern: "{controller=Home}/{action=StoryListing}");*/

app.Run();
