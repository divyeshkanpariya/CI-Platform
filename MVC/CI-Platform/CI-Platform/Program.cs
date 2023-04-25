using CI_Platform.Models.Models;
using CI_Platform.Repository.Interface;
using CI_Platform.Repository.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddControllersWithViews().AddMvcOptions(options => options.Filters.Add(new ResponseCacheAttribute { NoStore = true, Location = ResponseCacheLocation.None, Duration = 0 }));

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
builder.Services.AddScoped<IRepository<MissionInvite>, Repository<MissionInvite>>();
builder.Services.AddScoped<IRepository<Comment>, Repository<Comment>>();
builder.Services.AddScoped<IRepository<MissionApplication>, Repository<MissionApplication>>();
builder.Services.AddScoped<IRepository<MissionDocument>, Repository<MissionDocument>>();
builder.Services.AddScoped<IRepository<Story>, Repository<Story>>();
builder.Services.AddScoped<IRepository<StoryMedium>, Repository<StoryMedium>>();
builder.Services.AddScoped<IRepository<StoryInvite>, Repository<StoryInvite>>();
builder.Services.AddScoped<IRepository<UserSkill>, Repository<UserSkill>>();
builder.Services.AddScoped<IRepository<CmsPage>, Repository<CmsPage>>();
builder.Services.AddScoped<IRepository<ContactU>, Repository<ContactU>>();
builder.Services.AddScoped<IRepository<Timesheet>, Repository<Timesheet>>();
builder.Services.AddScoped<IRepository<Admin>, Repository<Admin>>();
builder.Services.AddScoped<IRepository<Banner>, Repository<Banner>>();


builder.Services.AddScoped<ILostPasswordRepository, LostPasswordRepository>();
builder.Services.AddScoped<ILoginRepository, LoginRepository>();
builder.Services.AddScoped<IResetPasswordRepository, ResetPasswordRepository>();
builder.Services.AddScoped<IRegistrationRepository, RegistrationRepository>();
builder.Services.AddScoped<IVolunteeringMissionRepository, VolunteeringMissionRepository>();
builder.Services.AddScoped<IFavouriteMission, FavouriteMissionRepository>();
builder.Services.AddScoped<IStoryListingRepository, StoryListingRepository>();
builder.Services.AddScoped<IStoryCardRepository, StoryCardRepository>();
builder.Services.AddScoped<IShareStoryRepository, ShareStoryRepository>();
builder.Services.AddScoped<IStoryDetailsRepository, StoryDetailsRepository>();
builder.Services.AddScoped<IUserProfileRepository, UserProfileRepository>();
builder.Services.AddScoped<IPrivacyPolicyRepository, PrivacyPolicyRepository>();
builder.Services.AddScoped<IContactUsRepository, ContactUsRepository>();
builder.Services.AddScoped<IVolunteeringTimesheet, VolunteeringTimesheetRepository>();

builder.Services.AddScoped<IMissionListingRepository, MissionListingRepository>();
builder.Services.AddScoped<IMissionCardRepository,MissionCardRepository>();

builder.Services.AddScoped<IAdminUserPageRepositoty,AdminUserPageRepository>();
builder.Services.AddScoped<IAdminCMSPageRepository,AdminCMSPageRepository>();
builder.Services.AddScoped<IAdminMissionPageRepository,AdminMissionPageRepository>();
builder.Services.AddScoped<IAdminMissionApplicationsRepository,AdminMissionApplicationsRepository>();
builder.Services.AddScoped<IAdminStoryRepository,AdminStoryRepository>();
builder.Services.AddScoped<IAdminSkillRepository,AdminSkillRepository>();
builder.Services.AddScoped<IAdminMissionThemeRepository,AdminMissionThemeRepository>();
builder.Services.AddScoped<IAdminBannerRepository,AdminBannerRepository>();


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
