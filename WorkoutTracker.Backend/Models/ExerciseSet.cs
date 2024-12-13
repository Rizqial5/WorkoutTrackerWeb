using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WorkoutTracker.Backend.Models
{
    public class ExerciseSet
    {
        [Key]
        public int ExerciseSetId { get; set; }

        [Required]
        public int Set { get; set; }

        [Required]
        public int Repetitions { get; set; }

        [Required]
        public string? Weight { get; set; }

        [ForeignKey("ExerciseId")]
        public int ExerciseId { get; set; }

        public ExerciseData? Exercise { get; set; }

    }
}
