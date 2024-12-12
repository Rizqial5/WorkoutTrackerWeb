using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace WorkoutTracker.Backend.Migrations
{
    /// <inheritdoc />
    public partial class AddMoreExercisess : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "ExerciseDatas",
                columns: new[] { "ExerciseId", "CategoryWorkout", "MuscleGroup", "Name", "PlansId" },
                values: new object[,]
                {
                    { 3, 1, 1, "Dumble Press", 0 },
                    { 4, 1, 3, "RDL (Romanian Deadlift)", 0 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "ExerciseDatas",
                keyColumn: "ExerciseId",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "ExerciseDatas",
                keyColumn: "ExerciseId",
                keyValue: 4);
        }
    }
}
