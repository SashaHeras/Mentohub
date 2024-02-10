using Mentohub.Domain.Data.Entities;
using Mentohub.Domain.Data.Entities.CourseEntities;
using Mentohub.Domain.Data.Entities.Order;
using Mentohub.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace Mentohub.Core.Context
{
    [Table("AspNetUsers")]
    public class ProjectContext : IdentityDbContext<CurrentUser>
    {
        public DbSet<Lesson> Lessons { get; set; }

        public DbSet<Test> Tests { get; set; }

        public DbSet<TaskAnswer> TaskAnswers { get; set; }

        public DbSet<TestTask> TestTasks { get; set; }

        public DbSet<TestHistory> TestHistory { get; set; }

        public DbSet<TaskHistory> TaskHistory { get; set; }

        public DbSet<AnswerHistory> AnswerHistory { get; set; }

        public DbSet<ItemStatus> ItemsStatuses { get; set; }

        public DbSet<Comment> Comments { get; set; }

        public DbSet<UserCourse> UserCourses { get; set; }

        #region Course

        public DbSet<Course> Courses { get; set; }

        public DbSet<CourseSubject> CourseSubjects { get; set; }

        public DbSet<CourseItem> CourseItem { get; set; }

        public DbSet<CourseItemType> CourseItemTypes { get; set; }

        public DbSet<CourseViews> CourseViews { get; set; }

        public DbSet<CourseBlock> CourseBlocks { get; set; }

        public DbSet<CourseLanguage> CourseLanguages { get; set; }

        public DbSet<CourseOverview> CourseOverviews { get; set; }

        public DbSet<CourseLevel> CourseLevel { get; set; }

        #endregion

        public DbSet<Order> Order { get; set; }

        public DbSet<OrderItem> OrderItem { get; set; }

        public DbSet<OrderPayment> OrderPayment { get; set; }

        public DbSet<Currency> Currency { get; set; }

        public ProjectContext(DbContextOptions<ProjectContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Lesson>().HasKey(x => x.Id);
            modelBuilder.Entity<Course>().HasKey(c => c.Id);
            modelBuilder.Entity<Test>().HasKey(c => c.Id);
            modelBuilder.Entity<CourseItem>().HasKey(c => c.id);
            modelBuilder.Entity<TaskAnswer>().HasKey(c => c.Id);
            modelBuilder.Entity<TestTask>().HasKey(c => c.Id);
            modelBuilder.Entity<AnswerHistory>().HasKey(c => c.Id);
            modelBuilder.Entity<TaskHistory>().HasKey(c => c.Id);
            modelBuilder.Entity<TestHistory>().HasKey(c => c.Id);
            modelBuilder.Entity<Comment>().HasKey(c => c.Id);
            modelBuilder.Entity<CourseSubject>().HasKey(c => c.Id);
            modelBuilder.Entity<CourseBlock>().HasKey(c => c.ID);
            modelBuilder.Entity<CourseViews>().HasKey(c => c.ID);
            modelBuilder.Entity<CourseLanguage>().HasKey(c => c.Id);
            modelBuilder.Entity<CourseOverview>().HasKey(c => c.ID);
            modelBuilder.Entity<CourseLevel>().HasKey(c => c.ID);
            modelBuilder.Entity<UserCourse>().HasKey(uc => uc.Id);
            modelBuilder.Entity<IdentityUserLogin<string>>().HasNoKey();
            modelBuilder.Entity<IdentityUserToken<string>>().HasNoKey();
            modelBuilder.Entity<IdentityUserRole<string>>().HasKey(ur => new { ur.UserId, ur.RoleId });

            modelBuilder.Entity<TestHistory>()
                .HasOne(t1 => t1.Test)
                .WithMany(t2 => t2.TestHistory)
                .HasForeignKey(t1 => t1.TestId);

            modelBuilder.Entity<TaskHistory>()
                .HasOne(t1 => t1.TestHistory)
                .WithMany(t2 => t2.TaskHistory)
                .HasForeignKey(t1 => t1.TestHistoryId);

            modelBuilder.Entity<TaskHistory>()
                .HasOne(t1 => t1.TestTask)
                .WithMany(t2 => t2.TaskHistory)
                .HasForeignKey(t1 => t1.TaskId);

            modelBuilder.Entity<AnswerHistory>()
                .HasOne(t1 => t1.TaskHistory)
                .WithMany(t2 => t2.AnswerHistory)
                .HasForeignKey(t1 => t1.TaskHistoryId);

            modelBuilder.Entity<TestTask>()
                .HasOne(c => c.Test)
                .WithMany(p => p.TestTasks)
                .HasForeignKey(c => c.TestId);

            modelBuilder.Entity<TaskAnswer>()
                .HasOne(t1 => t1.TestTask)
                .WithMany(t2 => t2.TaskAnswers)
                .HasForeignKey(t1 => t1.TaskId);

            modelBuilder.Entity<Lesson>()
                .HasOne(t1 => t1.CourseItem)
                .WithOne(t2 => t2.Lesson)
                .HasForeignKey<Lesson>(x => x.CourseItemId);

            modelBuilder.Entity<Test>()
                .HasOne(t1 => t1.CourseItem)
                .WithOne(t2 => t2.Test)
                .HasForeignKey<Test>(x => x.CourseItemId);

            modelBuilder.Entity<AnswerHistory>()
                .HasOne(t1 => t1.TaskAnswer)
                .WithMany(t2 => t2.AnswerHistory)
                .HasForeignKey(t1 => t1.AnswerId);

            #region Course

            modelBuilder.Entity<Course>()
               .HasOne(t1 => t1.CourseLevel)
               .WithMany(t2 => t2.Courses)
               .HasForeignKey(t1 => t1.CourseLevelID);

            modelBuilder.Entity<Course>()
               .HasOne(t1 => t1.Category)
               .WithMany(t2 => t2.Courses)
               .HasForeignKey(t1 => t1.CourseSubjectId);            

            modelBuilder.Entity<CourseViews>()
                .HasOne(t1 => t1.Course)
                .WithMany(t2 => t2.CourseViews)
                .HasForeignKey(x => x.CourseID);

            modelBuilder.Entity<CourseViews>()
                .HasOne(t1 => t1.User)
                .WithMany(t2 => t2.CourseViews)
                .HasForeignKey(x => x.UserID);

            modelBuilder.Entity<Course>()
                .HasOne(t1 => t1.Author)
                .WithMany(t2 => t2.Courses)
                .HasForeignKey(x => x.AuthorId);

            modelBuilder.Entity<Course>()
                .HasOne(t1 => t1.Language)
                .WithMany(t2 => t2.Courses)
                .HasForeignKey(x => x.LanguageID)
                .IsRequired(false);

            modelBuilder.Entity<CourseOverview>()
                .HasOne(t1 => t1.Course)
                .WithMany(t2 => t2.CourseOverviews)
                .HasForeignKey(x => x.CourseID)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<CourseTag>()
                .HasOne(t1 => t1.Course)
                .WithMany(t2 => t2.CourseTags)
                .HasForeignKey(x => x.CourseID)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<CourseTag>()
                .HasOne(t1 => t1.Tag)
                .WithMany(t2 => t2.CourseTags)
                .HasForeignKey(x => x.TagID)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Tag>()
                .HasOne(t1 => t1.User)
                .WithMany(t2 => t2.Tags)
                .HasForeignKey(x => x.UserID)
                .IsRequired(false);

            modelBuilder.Entity<Comment>()
                .HasOne(t1 => t1.Course)
                .WithMany(t2 => t2.Comments)
                .HasForeignKey(t1 => t1.CourseId);

            modelBuilder.Entity<CourseBlock>()
                .HasOne(t1 => t1.Course)
                .WithMany(t2 => t2.CourseBlocks)
                .HasForeignKey(t1 => t1.CourseID);

            modelBuilder.Entity<CourseItem>()
                .HasOne(t1 => t1.CourseBlock)
                .WithMany(t2 => t2.CourseItems)
                .HasForeignKey(t1 => t1.CourseBlockID);

            modelBuilder.Entity<Comment>()
                .HasOne(t1 => t1.User)
                .WithMany(t2 => t2.Comments)
                .HasForeignKey(t1 => t1.UserId);

            modelBuilder.Entity<CourseItem>()
                .HasOne(t1 => t1.Course)
                .WithMany(t2 => t2.CourseItems)
                .HasForeignKey(t1 => t1.CourseId);

            #endregion

            #region Payment

            modelBuilder.Entity<OrderPayment>()
               .HasOne(t1 => t1.Currency)
               .WithMany(t2 => t2.OrderPayments)
               .HasForeignKey(t1 => t1.CurrencyID);

            modelBuilder.Entity<OrderPayment>()
               .HasOne(t1 => t1.Order)
               .WithMany(t2 => t2.OrderPayments)
               .HasForeignKey(t1 => t1.OrderID);

            modelBuilder.Entity<OrderItem>()
               .HasOne(t1 => t1.Order)
               .WithMany(t2 => t2.OrderItems)
               .HasForeignKey(t1 => t1.OrderID);

            modelBuilder.Entity<UserCourse>()
               .HasOne(t1 => t1.OrderItem)
               .WithOne(t2 => t2.UserCourse)
               .HasForeignKey<UserCourse>(t1 => t1.OrderItemId);

            modelBuilder.Entity<UserCourse>()
               .HasOne(t1 => t1.OrderPayment)
               .WithMany(t2 => t2.UserCourses)
               .HasForeignKey(t1 => t1.OrderPaymentId);

            modelBuilder.Entity<Order>()
               .HasOne(t1 => t1.User)
               .WithMany(t2 => t2.Orders)
               .HasForeignKey(t1 => t1.UserID);

            modelBuilder.Entity<OrderItem>()
               .HasOne(t1 => t1.Course)
               .WithMany(t2 => t2.OrderItems)
               .HasForeignKey(t1 => t1.CourseID);

            #endregion

            modelBuilder.Entity<IdentityUserRole<string>>().ToTable("AspNetUserRoles");
            modelBuilder.Entity<IdentityUserLogin<string>>().ToTable("AspNetUserLogins");
            modelBuilder.Entity<IdentityRole>().ToTable("AspNetRoles");
            modelBuilder.Entity<IdentityUserToken<string>>().ToTable("AspNetUserTokens");
        }

        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    optionsBuilder.UseNpgsql("User ID=admin;Password=root;Host=34.118.18.52;Port=5432;Database=mentohub;MinPoolSize=0;MaxPoolSize=100;ConnectionLifetime=0;", builder =>
        //    {
        //        builder.EnableRetryOnFailure(5, TimeSpan.FromSeconds(10), null);
        //    });
        //}
    }
}
