using Mentohub.Core.AllExceptions;
using Mentohub.Core.Context;
using Mentohub.Core.Repositories.Repositories;
using Mentohub.Core.Services;
using Mentohub.Core.Services.Interfaces;
using Mentohub.Core.Services.Services;
using Mentohub.Domain.Data.Entities;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using Microsoft.Extensions.Configuration;


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
builder.Services.AddTransient<IEmailSender, EmailSender>();
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
//var connectionString = builder.Configuration["AzureServiceBus:ConnectionString"];
//var queueName = builder.Configuration["AzureServiceBus:QueueName"];
//builder.Services.AddSingleton<IQueueService>(provider =>
//{
   
//    return new QueueService(connectionString, queueName);
//});
builder.Services.AddSignalR();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Mentohub", Version = "v1" });

    // ���� � ����� ��� ���������� � ��
    c.TagActionsBy(api => new[] { api.GroupName });

    //��������� �� (HTTP ������)
    c.DocInclusionPredicate((docName, apiDesc) =>
    {
        if (!apiDesc.TryGetMethodInfo(out var methodInfo))
            return false;

        // ��������� HTTP ������
        if (docName == "v1" && methodInfo.CustomAttributes.Any(attr => attr.AttributeType == typeof(HttpGetAttribute)))
            return true;
        if (docName == "v1" && methodInfo.CustomAttributes.Any(attr => attr.AttributeType == typeof(HttpPostAttribute)))
            return true;
        if (docName == "v1" && methodInfo.CustomAttributes.Any(attr => attr.AttributeType == typeof(HttpPutAttribute)))
            return true;
        if (docName == "v1" && methodInfo.CustomAttributes.Any(attr => attr.AttributeType == typeof(HttpDeleteAttribute)))
            return true;
        return false;
    });
});

builder.Services.AddHttpContextAccessor();
builder.Services.AddControllers();

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
    app.UseCors();
    app.UseStaticFiles();
    
    app.UseDeveloperExceptionPage();
    app.UseHttpsRedirection();
    app.UseRouting();
    app.UseAuthorization();
app.UseEndpoints(endpoints =>
{
    
    endpoints.MapHub<SignalRHub>("/signalRHub"); 
});
    app.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}");

    app.Run();


