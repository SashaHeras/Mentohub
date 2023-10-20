using Mentohub.Core.AllExceptions;
using Mentohub.Core.Context;
using Mentohub.Core.Repositories.Repositories;
using Mentohub.Core.Services.Services;
using Mentohub.Domain.Data.Entities;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme,
        options => builder.Configuration.Bind("JwtSettings", options))
    .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme,
        options => builder.Configuration.Bind("CookieSettings", options));
builder.Services.AddAuthorization();
builder.Services.AddIdentity<CurrentUser, IdentityRole>()
    .AddEntityFrameworkStores<ProjectContext>()
    .AddDefaultTokenProviders();

builder.Services.AddDbContext<ProjectContext>(options => options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<AnswerHistoryRepository>();
builder.Services.AddScoped<AnswerRepository>();
builder.Services.AddScoped<CourseItemRepository>();
builder.Services.AddScoped<CourseRepository>();
builder.Services.AddScoped<CourseTypeRepository>();
builder.Services.AddScoped<LessonRepository>();
builder.Services.AddScoped<TaskHistoryRepository>();
builder.Services.AddScoped<TaskRepository>();
builder.Services.AddScoped<TestHistoryRepository>();
builder.Services.AddScoped<TestRepository>();
builder.Services.AddScoped<CRUD_UserRepository>();
builder.Services.AddScoped<AllException>();

builder.Services.AddScoped<AnswerHistoryService>();
builder.Services.AddScoped<AnswerService>();
builder.Services.AddScoped<AzureService>();
builder.Services.AddScoped<CourseItemService>();
builder.Services.AddScoped<CourseService>();
builder.Services.AddScoped<LessonService>();
builder.Services.AddScoped<MediaService>();
builder.Services.AddScoped<TaskHistoryService>();
builder.Services.AddScoped<TaskService>();
builder.Services.AddScoped<TestHistoryService>();
builder.Services.AddScoped<TestService>();
builder.Services.AddScoped<UserService>();
builder.Services.AddSignalR();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Mentohub", Version = "v1" });
});
var app = builder.Build();
app.UseAuthentication();
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Mentohub6" + " V1");
});
// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseStaticFiles();
app.UseDeveloperExceptionPage();
app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
