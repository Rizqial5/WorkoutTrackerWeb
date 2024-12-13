using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WorkoutTracker.Backend.Migrations
{
    /// <inheritdoc />
    public partial class SchedulePlansData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ScheduledTime",
                table: "WorkoutPlans");

            migrationBuilder.CreateTable(
                name: "SchedulePlans",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ScheduleTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    WorkoutPlansId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SchedulePlans", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SchedulePlans_WorkoutPlans_WorkoutPlansId",
                        column: x => x.WorkoutPlansId,
                        principalTable: "WorkoutPlans",
                        principalColumn: "PlanId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SchedulePlans_WorkoutPlansId",
                table: "SchedulePlans",
                column: "WorkoutPlansId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SchedulePlans");

            migrationBuilder.AddColumn<DateTime>(
                name: "ScheduledTime",
                table: "WorkoutPlans",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }
    }
}
