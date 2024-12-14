﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WorkoutTracker.Backend.Migrations
{
    /// <inheritdoc />
    public partial class ChangePlanStatusClass : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PlanStatus",
                table: "WorkoutPlans");

            migrationBuilder.AddColumn<int>(
                name: "PlanStatus",
                table: "SchedulePlans",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PlanStatus",
                table: "SchedulePlans");

            migrationBuilder.AddColumn<int>(
                name: "PlanStatus",
                table: "WorkoutPlans",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}