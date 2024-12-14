using System.ComponentModel.DataAnnotations;

namespace WorkoutTracker.Backend.Models
{
    public class SchedulePlans
    {
        [Key]
        public int Id { get; set; }

        public DateTime ScheduleTime { get; set; }

        public PlanStatus PlanStatus { get; set; }

        public int WorkoutPlansId { get; set; }
        //Navigation Property
        public WorkoutPlans WorkoutPlan { get; set; }
    }

    public enum PlanStatus
    {
        Active,
        Done,
    }
}
