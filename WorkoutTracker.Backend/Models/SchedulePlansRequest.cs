namespace WorkoutTracker.Backend.Models
{
    public class SchedulePlansRequest
    {
        public int Id { get; set; }
        public DateTime DateTime { get; set; }
        public PlanStatus PlanStatus { get; set; }
        public int WorkoutPlansId { get; set; }
    }
}
