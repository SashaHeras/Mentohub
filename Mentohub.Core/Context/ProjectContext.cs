using Mentohub.Domain.Data.Entities;
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

        public DbSet<Course> Courses { get; set; }

        public DbSet<CourseSubject> CourseSubjects { get; set; }

        public DbSet<CourseItem> CourseItem { get; set; }

        public DbSet<CourseItemType> CourseItemTypes { get; set; }

        public DbSet<TestHistory> TestHistory { get; set; }

        public DbSet<TaskHistory> TaskHistory { get; set; }

        public DbSet<AnswerHistory> AnswerHistory { get; set; }

        public DbSet<ItemStatus> ItemsStatuses { get; set; }

        public DbSet<Comment> Comments { get; set; }

        public ProjectContext(DbContextOptions<ProjectContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Lesson>().HasKey(x => x.Id);
            modelBuilder.Entity<Course>().HasKey(c => c.Id);
            modelBuilder.Entity<Test>().HasKey(c => c.Id);
            modelBuilder.Entity<CourseItem>().HasKey(c => c.Id);
            modelBuilder.Entity<TaskAnswer>().HasKey(c => c.Id);
            modelBuilder.Entity<TestTask>().HasKey(c => c.Id);
            modelBuilder.Entity<AnswerHistory>().HasKey(c => c.Id);
            modelBuilder.Entity<TaskHistory>().HasKey(c => c.Id);
            modelBuilder.Entity<TestHistory>().HasKey(c => c.Id);
            modelBuilder.Entity<IdentityUserLogin<string>>().HasNoKey();
            modelBuilder.Entity<IdentityUserToken<string>>().HasNoKey();
            modelBuilder.Entity<IdentityUserRole<string>>().HasKey(ur => new { ur.UserId, ur.RoleId });

            modelBuilder.Entity<TestHistory>()
                .HasOne(t1 => t1.Test)
                .WithMany(t2 => t2.TestHistory)
                .HasForeignKey(t1 => t1.TestId);

            modelBuilder.Entity<CourseItem>()
                .HasOne(t1 => t1.Course)
                .WithMany(t2 => t2.CourseItems)
                .HasForeignKey(t1 => t1.CourseId);

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

            modelBuilder.Entity<CourseItem>()
                .HasOne(t1 => t1.Course)
                .WithMany(t2 => t2.CourseItems)
                .HasForeignKey(t1 => t1.CourseId);

            modelBuilder.Entity<TestTask>()
                .HasOne(c => c.Test)
                .WithMany(p => p.TestTasks)
                .HasForeignKey(c => c.TestId);

            modelBuilder.Entity<TaskAnswer>()
                .HasOne(t1 => t1.TestTask)
                .WithMany(t2 => t2.TaskAnswers)
                .HasForeignKey(t1 => t1.TaskId);

            modelBuilder.Entity<AnswerHistory>()
                .HasOne(t1 => t1.TaskAnswer)
                .WithMany(t2 => t2.AnswerHistory)
                .HasForeignKey(t1 => t1.AnswerId);

            modelBuilder.Entity<IdentityUserRole<string>>().ToTable("AspNetUserRoles");
            modelBuilder.Entity<IdentityUserLogin<string>>().ToTable("AspNetUserLogins");
            modelBuilder.Entity<IdentityRole>().ToTable("AspNetRoles");
            modelBuilder.Entity<IdentityUserToken<string>>().ToTable("AspNetUserTokens");
        }

        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    //"Server=ALLA2021\\SQLEXPRESS01;Database=StudyDB;Trusted_Connection=True;MultipleActiveResultSets=true"
        //    //"Server=localhost;Database=StudyDB; Trusted_Connection=True;TrustServerCertificate=True"
        //    //optionsBuilder.UseSqlServer("Server=ALLA2021\\SQLEXPRESS01;Database=StudyDB;Trusted_Connection=True;MultipleActiveResultSets=true", builder =>
        //    //{
        //    //    builder.EnableRetryOnFailure(5, TimeSpan.FromSeconds(10), null);
        //    //});
        //}
        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    optionsBuilder.UseNpgsql("User ID=admin;Password=root;Host=localhost;Port=5432;Database=mentohub;Pooling=true;MinPoolSize=0;MaxPoolSize=100;ConnectionLifetime=0;", builder =>
        //    {
        //        builder.EnableRetryOnFailure(5, TimeSpan.FromSeconds(10), null);
        //    });
        //}
    }
}
