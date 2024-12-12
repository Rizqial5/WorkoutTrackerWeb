namespace WorkoutTracker.Backend.Models
{
    public class WorkoutPlansPostDTO
    {
        public string PlansName { get; set; }
        public ICollection<int> ExercisesCollection { get; set; }

    }
}
