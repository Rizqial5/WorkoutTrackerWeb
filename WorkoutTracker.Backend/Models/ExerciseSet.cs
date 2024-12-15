using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace WorkoutTracker.Backend.Models
{
    public class ExerciseSet
    {
        [Key]
        public int ExerciseSetId { get; set; }

        [Required]
        [StringLength(50)]
        public string ExerciseSetName { get; set; }

        [Required]
        public int Set { get; set; }

        [Required]
        public int Repetitions { get; set; }

        [Required]
        [StringLength(50)]
        public string? Weight { get; set; }

        [ForeignKey("ExerciseId")]
        public int ExerciseId { get; set; }
        public ExerciseData? Exercise { get; set; }

        [ForeignKey("PlansId")]
        public ICollection<WorkoutPlans> WorkoutPlans { get; set; }

        [ForeignKey("UserId")]
        public string UserId { get; set; }
        public IdentityUser User { get; set; }

    }
}
