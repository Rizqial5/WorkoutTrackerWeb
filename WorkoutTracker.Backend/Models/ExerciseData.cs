

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

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

        [JsonIgnore]
        public ICollection<ExerciseSet> ExerciseSets { get; set; }
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
