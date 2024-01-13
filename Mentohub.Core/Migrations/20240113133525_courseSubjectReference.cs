using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Mentohub.Core.Migrations
{
    public partial class courseSubjectReference : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Courses_CourseSubjectId",
                table: "Courses",
                column: "CourseSubjectId");

            migrationBuilder.AddForeignKey(
                name: "FK_Courses_CourseSubjects_CourseSubjectId",
                table: "Courses",
                column: "CourseSubjectId",
                principalTable: "CourseSubjects",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Courses_CourseSubjects_CourseSubjectId",
                table: "Courses");

            migrationBuilder.DropIndex(
                name: "IX_Courses_CourseSubjectId",
                table: "Courses");
        }
    }
}
