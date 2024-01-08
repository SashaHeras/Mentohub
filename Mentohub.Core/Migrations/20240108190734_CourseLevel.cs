using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Mentohub.Core.Migrations
{
    public partial class CourseLevel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CourseLevelID",
                table: "Courses",
                type: "integer",
                nullable: true,
                defaultValue: null);

            migrationBuilder.CreateTable(
                name: "CourseLevel",
                columns: table => new
                {
                    ID = table.Column<int>(type: "integer", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CourseLevel", x => x.ID);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Courses_CourseLevelID",
                table: "Courses",
                column: "CourseLevelID");

            migrationBuilder.AddForeignKey(
                name: "FK_Courses_CourseLevel_CourseLevelID",
                table: "Courses",
                column: "CourseLevelID",
                principalTable: "CourseLevel",
                principalColumn: "ID",
                onDelete: ReferentialAction.NoAction);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Courses_CourseLevel_CourseLevelID",
                table: "Courses");

            migrationBuilder.DropTable(
                name: "CourseLevel");

            migrationBuilder.DropIndex(
                name: "IX_Courses_CourseLevelID",
                table: "Courses");

            migrationBuilder.DropColumn(
                name: "CourseLevelID",
                table: "Courses");
        }
    }
}
