namespace WorkoutTracker.Backend.Models
{
    public class WorkoutPlanResponse
    {
        public int PlanId { get; set; }
        public string PlanName { get; set; }
        public DateTime? ScheduledTime { get; set; }
        public List<ExerciseSetResponse> ExerciseSets { get; set; }
    }
}
