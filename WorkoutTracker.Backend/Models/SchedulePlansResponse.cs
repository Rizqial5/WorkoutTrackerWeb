namespace WorkoutTracker.Backend.Models
{
    public class SchedulePlansResponse
    {
        public int Id { get; set; }
        public DateTime PlannedDateTime { get; set; }
        public WorkoutPlanResponse WorkoutPlanResponse { get; set; }
    }
}
