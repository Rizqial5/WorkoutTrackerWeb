using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace WorkoutTracker.Backend.Migrations
{
    /// <inheritdoc />
    public partial class Init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ExerciseDatas",
                columns: table => new
                {
                    ExerciseId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CategoryWorkout = table.Column<int>(type: "int", nullable: false),
                    MuscleGroup = table.Column<int>(type: "int", nullable: false),
                    PlansId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExerciseDatas", x => x.ExerciseId);
                });

            migrationBuilder.CreateTable(
                name: "WorkoutPlans",
                columns: table => new
                {
                    PlanId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PlanName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkoutPlans", x => x.PlanId);
                });

            migrationBuilder.CreateTable(
                name: "ExerciseDataWorkoutPlans",
                columns: table => new
                {
                    PlansId = table.Column<int>(type: "int", nullable: false),
                    WorkoutPlansPlanId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExerciseDataWorkoutPlans", x => new { x.PlansId, x.WorkoutPlansPlanId });
                    table.ForeignKey(
                        name: "FK_ExerciseDataWorkoutPlans_ExerciseDatas_PlansId",
                        column: x => x.PlansId,
                        principalTable: "ExerciseDatas",
                        principalColumn: "ExerciseId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ExerciseDataWorkoutPlans_WorkoutPlans_WorkoutPlansPlanId",
                        column: x => x.WorkoutPlansPlanId,
                        principalTable: "WorkoutPlans",
                        principalColumn: "PlanId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "ExerciseDatas",
                columns: new[] { "ExerciseId", "CategoryWorkout", "MuscleGroup", "Name", "PlansId" },
                values: new object[,]
                {
                    { 1, 1, 1, "Bench Press", 0 },
                    { 2, 1, 3, "Squat", 0 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_ExerciseDataWorkoutPlans_WorkoutPlansPlanId",
                table: "ExerciseDataWorkoutPlans",
                column: "WorkoutPlansPlanId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ExerciseDataWorkoutPlans");

            migrationBuilder.DropTable(
                name: "ExerciseDatas");

            migrationBuilder.DropTable(
                name: "WorkoutPlans");
        }
    }
}
