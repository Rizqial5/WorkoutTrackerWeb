using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WorkoutTracker.Backend.Migrations
{
    /// <inheritdoc />
    public partial class AddDatabaseRelation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "SchedulePlans",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "ExerciseSets",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_SchedulePlans_UserId",
                table: "SchedulePlans",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_ExerciseSets_UserId",
                table: "ExerciseSets",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_ExerciseSets_AspNetUsers_UserId",
                table: "ExerciseSets",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SchedulePlans_AspNetUsers_UserId",
                table: "SchedulePlans",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ExerciseSets_AspNetUsers_UserId",
                table: "ExerciseSets");

            migrationBuilder.DropForeignKey(
                name: "FK_SchedulePlans_AspNetUsers_UserId",
                table: "SchedulePlans");

            migrationBuilder.DropIndex(
                name: "IX_SchedulePlans_UserId",
                table: "SchedulePlans");

            migrationBuilder.DropIndex(
                name: "IX_ExerciseSets_UserId",
                table: "ExerciseSets");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "SchedulePlans");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "ExerciseSets");
        }
    }
}
