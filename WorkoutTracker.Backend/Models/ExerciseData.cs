

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WorkoutTracker.Backend.Models
{
    public class ExerciseData
    {
        [Key]
        public int ExerciseId { get; set; }

        [Required]
        public string? Name { get; set; }
        [Required]
        public CategoryWorkout CategoryWorkout { get; set; }
        [Required]
        public MuscleGroup MuscleGroup { get; set; }

        [ForeignKey("PlansId")]
        public ICollection<WorkoutPlans> WorkoutPlans { get; set; }
        public int PlansId { get; set; }
    }

    public enum CategoryWorkout
    {
        Cardio,
        Strength,
        Flexibility,
    }

    public enum MuscleGroup
    {
        Back,
        Chest,
        Arm,
        Leg,
        HeartRate
    }
}
