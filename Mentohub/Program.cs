using Mentohub.Core.AllExceptions;
using Mentohub.Core.Context;
using Mentohub.Core.Repositories.Intefaces;
using Mentohub.Core.Repositories.Interfaces;
using Mentohub.Core.Repositories.Repositories;
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
using Mentohub.Core.Services;
using Mentohub.Core.Repositories.Interfaces.CourseInterfaces;
using Mentohub.Core.Repositories.Repositories.CourseRepositories;
using Mentohub.Core.Repositories.Interfaces.PaymentInterfaces;
using Mentohub.Core.Repositories.Repositories.PaymentRepository;
using Mentohub.Core.Services.Services.PaymentServices;

internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

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

        AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

        builder.Services.AddDbContextPool<ProjectContext>(
                options => options.UseLazyLoadingProxies()
                                  .UseNpgsql(builder.Configuration.GetConnectionString("DefaultPost"))
        );

        builder.Services.AddCors(co =>
        {
            co.AddPolicy("MentoPolicy", p =>
            {
                p.WithOrigins("http://34.116.248.113", "https://localhost:7236", "https://behm.net")
                 .AllowAnyMethod()
                 .AllowCredentials()
                 .AllowAnyHeader();
            });
        });

        builder.Services.AddScoped<IAnswerHistoryRepository, AnswerHistoryRepository>();
        builder.Services.AddScoped<IAnswerRepository, AnswerRepository>();
        builder.Services.AddScoped<ICourseItemRepository, CourseItemRepository>();
        builder.Services.AddScoped<ICourseRepository, CourseRepository>();
        builder.Services.AddScoped<ICourseTypeRepository, CourseTypeRepository>();
        builder.Services.AddScoped<ILessonRepository, LessonRepository>();
        builder.Services.AddScoped<ITaskHistoryRepository, TaskHistoryRepository>();
        builder.Services.AddScoped<ITaskRepository, TaskRepository>();
        builder.Services.AddScoped<ITestHistoryRepository, TestHistoryRepository>();
        builder.Services.AddScoped<ITestRepository, TestRepository>();
        builder.Services.AddScoped<ICommentRepository, CommentRepository>();
        builder.Services.AddScoped<ICourseViewsRepository, CourseViewsRepository>();
        builder.Services.AddScoped<ICRUD_UserRepository, CRUD_UserRepository>();
        builder.Services.AddScoped<ISubjectRepository, SubjectRepository>();
        builder.Services.AddScoped<ICourseBlockRepository, CourseBlockRepository>();
        builder.Services.AddScoped<ICourseLanguageRepository, CourseLanguageRepository>();
        builder.Services.AddScoped<ICourseOverviewRepository, CourseOverviewRepository>();
        builder.Services.AddScoped<ICourseLevelRepository, CourseLevelRepository>();
        builder.Services.AddScoped<IOrderRepository, OrderRepository>();
        builder.Services.AddScoped<IUserCourseRepository, UserCourseRepository>();
        builder.Services.AddScoped<IOrderItemRepository, OrderItemRepository>();
        builder.Services.AddScoped<IOrderPaymentRepository, OrderPaymentRepository>();
        builder.Services.AddScoped<AllException>();

        builder.Services.AddScoped<IAnswerHistoryService, AnswerHistoryService>();
        builder.Services.AddScoped<IAnswerService, AnswerService>();
        builder.Services.AddScoped<IAzureService, AzureService>();
        builder.Services.AddScoped<ICourseItemService, CourseItemService>();
        builder.Services.AddScoped<ICourseService, CourseService>();
        builder.Services.AddScoped<ICommentService, CommentService>();        
        builder.Services.AddScoped<ILessonService, LessonService>();
        builder.Services.AddScoped<IMediaService, MediaService>();
        builder.Services.AddScoped<ITaskHistoryService, TaskHistoryService>();
        builder.Services.AddScoped<ITaskService, TaskService>();
        builder.Services.AddScoped<ITestHistoryService, TestHistoryService>();
        builder.Services.AddScoped<ITestService, TestService>();
        builder.Services.AddScoped<IUserService, UserService>();
        builder.Services.AddScoped<ICourseViewService, CourseViewService>();
        builder.Services.AddScoped<ICourseBlockService, CourseBlockService>();
        builder.Services.AddScoped<IOverviewService, OverviewService>();
        builder.Services.AddScoped<ICourseLevelService, CourseLevelService>();
        builder.Services.AddScoped<ICourseSubjectService, CourseSubjectService>();
        builder.Services.AddScoped<ICourseLanguageService, CourseLanguageService>();
        builder.Services.AddTransient<IEmailSender, EmailSender>();
        builder.Services.AddHttpClient<IPrivatBankService, PrivatBankService>();
        builder.Services.AddScoped<IOrderService, OrderService>();
        builder.Services.AddScoped<IUserCourseService, UserCourseService>();
        builder.Services.AddScoped<IOrderItemSevice, OrderItemSevice>();

        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSignalR();
        builder.Services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "Mentohub", Version = "v1" });

            // теги і описи для контролерів і дій
            c.TagActionsBy(api => new[] { api.GroupName });

            //параметри дій (HTTP методи)
            c.DocInclusionPredicate((docName, apiDesc) =>
            {
                if (!apiDesc.TryGetMethodInfo(out var methodInfo))
                    return false;

                // Перевірити HTTP методи
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

        app.UseStaticFiles();

        app.UseDeveloperExceptionPage();
        app.UseHttpsRedirection();
        app.UseRouting();
        app.UseCors();
        app.UseAuthorization();
        app.UseEndpoints(endpoints =>
        {
            endpoints.MapHub<SignalRHub>("/signalRHub");
        });
        app.MapControllerRoute(
            name: "default",
            pattern: "{controller=Home}/{action=Index}/{id?}");

        app.Run();
    }
}