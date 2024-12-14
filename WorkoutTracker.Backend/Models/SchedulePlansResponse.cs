namespace WorkoutTracker.Backend.Models
{
    public class SchedulePlansResponse
    {
        public int Id { get; set; }
        public DateTime PlannedDateTime { get; set; }
        public PlanStatus PlanStatus { get; set; }
        public int PlanId { get; set; }
        public string PlanName { get; set; }
    }
}
