using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WorkoutTracker.Backend.Migrations
{
    /// <inheritdoc />
    public partial class NewExerciseSetData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ExerciseDataWorkoutPlans");

            migrationBuilder.RenameColumn(
                name: "PlansId",
                table: "ExerciseDatas",
                newName: "ExerciseSetId");

            migrationBuilder.CreateTable(
                name: "ExerciseSets",
                columns: table => new
                {
                    ExerciseSetId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ExerciseSetName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Set = table.Column<int>(type: "int", nullable: false),
                    Repetitions = table.Column<int>(type: "int", nullable: false),
                    Weight = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ExerciseId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExerciseSets", x => x.ExerciseSetId);
                    table.ForeignKey(
                        name: "FK_ExerciseSets_ExerciseDatas_ExerciseId",
                        column: x => x.ExerciseId,
                        principalTable: "ExerciseDatas",
                        principalColumn: "ExerciseId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ExerciseSetWorkoutPlans",
                columns: table => new
                {
                    PlansId = table.Column<int>(type: "int", nullable: false),
                    WorkoutPlansPlanId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExerciseSetWorkoutPlans", x => new { x.PlansId, x.WorkoutPlansPlanId });
                    table.ForeignKey(
                        name: "FK_ExerciseSetWorkoutPlans_ExerciseSets_PlansId",
                        column: x => x.PlansId,
                        principalTable: "ExerciseSets",
                        principalColumn: "ExerciseSetId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ExerciseSetWorkoutPlans_WorkoutPlans_WorkoutPlansPlanId",
                        column: x => x.WorkoutPlansPlanId,
                        principalTable: "WorkoutPlans",
                        principalColumn: "PlanId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ExerciseSets_ExerciseId",
                table: "ExerciseSets",
                column: "ExerciseId");

            migrationBuilder.CreateIndex(
                name: "IX_ExerciseSetWorkoutPlans_WorkoutPlansPlanId",
                table: "ExerciseSetWorkoutPlans",
                column: "WorkoutPlansPlanId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ExerciseSetWorkoutPlans");

            migrationBuilder.DropTable(
                name: "ExerciseSets");

            migrationBuilder.RenameColumn(
                name: "ExerciseSetId",
                table: "ExerciseDatas",
                newName: "PlansId");

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

            migrationBuilder.CreateIndex(
                name: "IX_ExerciseDataWorkoutPlans_WorkoutPlansPlanId",
                table: "ExerciseDataWorkoutPlans",
                column: "WorkoutPlansPlanId");
        }
    }
}
