using CI_Project.Entities.DataModels;
using CI_Project.Entities.ViewModels;
using CI_Project.Repository.Repository;
using CI_Project.Repository.Repository.Interface;
using CI_Project.Services;
using CI_Project.Services.Interface;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddDbContext<CIProjectDbContext>();
builder.Services.AddScoped<IUnitOfService,UnitOfService>();

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IHomeRepository, HomeRepository>();
builder.Services.AddScoped<IVolunteeringMissionRepository, VolunteeringMissionRepository>();
builder.Services.AddScoped<IStoryRepository, StoryRepository>();
builder.Services.AddScoped<ICmsRepository, CmsRepository>();
builder.Services.AddScoped<ISkillsRepository, SkillRepository>();
builder.Services.AddScoped<IMissionsSkills, MissionsSkills>();
builder.Services.AddScoped<IUsersSkill, UsersSkill>();
builder.Services.AddScoped<IMissionApplication, MissionApplications>();
builder.Services.AddScoped<IMissionThemeRepository, MissionThemeRepository>();
builder.Services.AddScoped<IMissionRepository, MissionRepository>();
builder.Services.AddScoped<IMissionMediaRepository, MissionMediaRepository>();
builder.Services.AddScoped<IMissionDocument, MissionDocumentRepository>();
builder.Services.AddScoped<IGoalMissionRepository,GoalMissionRepositor>();




builder.Services.AddDistributedMemoryCache();

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(100);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
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

app.UseAuthorization();
app.UseSession();


app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
