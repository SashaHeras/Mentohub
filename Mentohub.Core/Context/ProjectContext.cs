using Mentohub.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace Mentohub.Core.Context
{
    public class ProjectContext : DbContext
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

        public ProjectContext()
        {

        }

        public ProjectContext(DbContextOptions<ProjectContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Lesson>().HasKey(x => x.Id);
            modelBuilder.Entity<Course>().HasKey(c => c.Id);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=localhost;Initial Catalog=StudyDB;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False;Trusted_Connection=True", builder =>
            {
                builder.EnableRetryOnFailure(5, TimeSpan.FromSeconds(10), null);
            });
            base.OnConfiguring(optionsBuilder);
        }
    }
}
