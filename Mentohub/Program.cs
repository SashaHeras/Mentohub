using Mentohub.Core.Context;
using Mentohub.Core.Repositories.Repositories;
using Mentohub.Core.Services.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<ProjectContext>(options => options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")
        ));

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

var app = builder.Build();

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
