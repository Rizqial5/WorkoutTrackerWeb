using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WorkoutTracker.Backend.Migrations
{
    /// <inheritdoc />
    public partial class FixRelationshipData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ExerciseSetId",
                table: "ExerciseDatas");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ExerciseSetId",
                table: "ExerciseDatas",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "ExerciseDatas",
                keyColumn: "ExerciseId",
                keyValue: 1,
                column: "ExerciseSetId",
                value: 0);

            migrationBuilder.UpdateData(
                table: "ExerciseDatas",
                keyColumn: "ExerciseId",
                keyValue: 2,
                column: "ExerciseSetId",
                value: 0);

            migrationBuilder.UpdateData(
                table: "ExerciseDatas",
                keyColumn: "ExerciseId",
                keyValue: 3,
                column: "ExerciseSetId",
                value: 0);

            migrationBuilder.UpdateData(
                table: "ExerciseDatas",
                keyColumn: "ExerciseId",
                keyValue: 4,
                column: "ExerciseSetId",
                value: 0);
        }
    }
}
