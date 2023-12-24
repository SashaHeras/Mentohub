using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Mentohub.Core.Migrations
{
    public partial class add_user_views_course : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Tests_CourseItemId",
                table: "Tests");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "TestHistory",
                type: "text",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.CreateTable(
                name: "CourseViews",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uuid", nullable: false),
                    ViewDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    UserID = table.Column<string>(type: "text", nullable: false),
                    CourseID = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CourseViews", x => x.ID);
                    table.ForeignKey(
                        name: "FK_CourseViews_AspNetUsers_UserID",
                        column: x => x.UserID,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CourseViews_Courses_CourseID",
                        column: x => x.CourseID,
                        principalTable: "Courses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Tests_CourseItemId",
                table: "Tests",
                column: "CourseItemId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TestHistory_UserId",
                table: "TestHistory",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Lessons_CourseItemId",
                table: "Lessons",
                column: "CourseItemId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CourseViews_CourseID",
                table: "CourseViews",
                column: "CourseID");

            migrationBuilder.CreateIndex(
                name: "IX_CourseViews_UserID",
                table: "CourseViews",
                column: "UserID");

            migrationBuilder.AddForeignKey(
                name: "FK_Lessons_CourseItem_CourseItemId",
                table: "Lessons",
                column: "CourseItemId",
                principalTable: "CourseItem",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TestHistory_AspNetUsers_UserId",
                table: "TestHistory",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Lessons_CourseItem_CourseItemId",
                table: "Lessons");

            migrationBuilder.DropForeignKey(
                name: "FK_TestHistory_AspNetUsers_UserId",
                table: "TestHistory");

            migrationBuilder.DropTable(
                name: "CourseViews");

            migrationBuilder.DropIndex(
                name: "IX_Tests_CourseItemId",
                table: "Tests");

            migrationBuilder.DropIndex(
                name: "IX_TestHistory_UserId",
                table: "TestHistory");

            migrationBuilder.DropIndex(
                name: "IX_Lessons_CourseItemId",
                table: "Lessons");

            migrationBuilder.AlterColumn<Guid>(
                name: "UserId",
                table: "TestHistory",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.CreateIndex(
                name: "IX_Tests_CourseItemId",
                table: "Tests",
                column: "CourseItemId");
        }
    }
}
