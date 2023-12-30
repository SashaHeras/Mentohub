using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Mentohub.Core.Migrations
{
    public partial class addLanguageCourse : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "LanguageID",
                table: "Courses",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "CourseLanguages",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CourseLanguages", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Courses_LanguageID",
                table: "Courses",
                column: "LanguageID");

            migrationBuilder.AddForeignKey(
                name: "FK_Courses_CourseLanguages_LanguageID",
                table: "Courses",
                column: "LanguageID",
                principalTable: "CourseLanguages",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Courses_CourseLanguages_LanguageID",
                table: "Courses");

            migrationBuilder.DropTable(
                name: "CourseLanguages");

            migrationBuilder.DropIndex(
                name: "IX_Courses_LanguageID",
                table: "Courses");

            migrationBuilder.DropColumn(
                name: "LanguageID",
                table: "Courses");
        }
    }
}
