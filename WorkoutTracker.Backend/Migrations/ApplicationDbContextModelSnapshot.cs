﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace WorkoutTracker.Backend.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    partial class ApplicationDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("ExerciseDataWorkoutPlans", b =>
                {
                    b.Property<int>("PlansId")
                        .HasColumnType("int");

                    b.Property<int>("WorkoutPlansPlanId")
                        .HasColumnType("int");

                    b.HasKey("PlansId", "WorkoutPlansPlanId");

                    b.HasIndex("WorkoutPlansPlanId");

                    b.ToTable("ExerciseDataWorkoutPlans");
                });

            modelBuilder.Entity("WorkoutTracker.Backend.Models.ExerciseData", b =>
                {
                    b.Property<int>("ExerciseId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ExerciseId"));

                    b.Property<int>("CategoryWorkout")
                        .HasColumnType("int");

                    b.Property<int>("MuscleGroup")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("PlansId")
                        .HasColumnType("int");

                    b.HasKey("ExerciseId");

                    b.ToTable("ExerciseDatas");

                    b.HasData(
                        new
                        {
                            ExerciseId = 1,
                            CategoryWorkout = 1,
                            MuscleGroup = 1,
                            Name = "Bench Press",
                            PlansId = 0
                        },
                        new
                        {
                            ExerciseId = 2,
                            CategoryWorkout = 1,
                            MuscleGroup = 3,
                            Name = "Squat",
                            PlansId = 0
                        });
                });

            modelBuilder.Entity("WorkoutTracker.Backend.Models.WorkoutPlans", b =>
                {
                    b.Property<int>("PlanId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("PlanId"));

                    b.Property<string>("PlanName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("PlanId");

                    b.ToTable("WorkoutPlans");
                });

            modelBuilder.Entity("ExerciseDataWorkoutPlans", b =>
                {
                    b.HasOne("WorkoutTracker.Backend.Models.ExerciseData", null)
                        .WithMany()
                        .HasForeignKey("PlansId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("WorkoutTracker.Backend.Models.WorkoutPlans", null)
                        .WithMany()
                        .HasForeignKey("WorkoutPlansPlanId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
