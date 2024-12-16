namespace WorkoutTracker.Backend.Models
{
    public class WorkoutPlanReport
    {
        public string PlanName { get; set; }
        public List<string> ExerciseListName { get; set; }
        public int TotalWorkoutDone { get; set; }
        public List<ScheduleDone> ListPastSchedule { get; set; }
    }

    public class ScheduleDone
    {
        public DateTime DateSchedule { get; set; }
        public PlanStatus PlanStatus { get; set; }
    }
}
