using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WorkoutTracker.Backend.Models;

    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext (DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<WorkoutPlans>(entity =>
            {
                entity.HasKey(w => w.PlanId);

                entity.HasMany(wp => wp.ScheduledTime)
                    .WithOne(sp => sp.WorkoutPlan)
                    .HasForeignKey(wp => wp.WorkoutPlansId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasMany(e => e.ExerciseSets)
                    .WithMany(w => w.WorkoutPlans);
            });



            modelBuilder.Entity<ExerciseSet>(entity =>
            {
                entity.HasKey(s => s.ExerciseSetId);

                entity.HasOne(s => s.Exercise)
                    .WithMany(e => e.ExerciseSets)
                    .HasForeignKey(s => s.ExerciseId)
                    .OnDelete(DeleteBehavior.Cascade);


            });



            modelBuilder.Entity<ExerciseData>().HasData(

                new ExerciseData { ExerciseId = 1, Name = "Bench Press", CategoryWorkout = CategoryWorkout.Strength, MuscleGroup = MuscleGroup.Chest},
                new ExerciseData { ExerciseId = 2, Name = "Squat", CategoryWorkout = CategoryWorkout.Strength, MuscleGroup = MuscleGroup.Leg},
                new ExerciseData { ExerciseId = 3, Name = "Dumble Press", CategoryWorkout = CategoryWorkout.Strength, MuscleGroup = MuscleGroup.Chest},
                new ExerciseData { ExerciseId = 4, Name = "RDL (Romanian Deadlift)", CategoryWorkout = CategoryWorkout.Strength, MuscleGroup = MuscleGroup.Leg}
                
            );
        }

        public DbSet<WorkoutPlans> WorkoutPlans { get; set; } = default!;
        public DbSet<ExerciseData> ExerciseDatas { get; set; } = default!;
        public DbSet<ExerciseSet> ExerciseSets { get; set; } = default!;
    }
