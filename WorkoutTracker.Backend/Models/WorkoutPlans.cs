using System.ComponentModel.DataAnnotations;

namespace WorkoutTracker.Backend.Models
{
    public class WorkoutPlans
    {
        [Key]
        public int PlanId { get; set; }

        [Required]
        [StringLength(50)]
        public string PlanName { get; set; }

        
        public DateTime ScheduledTime { get; set; }

        
        public ICollection<ExerciseSet> ExerciseSets { get; set; }

    }
}
