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

        public PlanStatus PlanStatus { get; set; }

        
        public ICollection<SchedulePlans> ScheduledTime { get; set; }

        
        public ICollection<ExerciseSet> ExerciseSets { get; set; }

    }

    public enum PlanStatus
    {
        Active,
        Pending,
        Done,
    }
}
