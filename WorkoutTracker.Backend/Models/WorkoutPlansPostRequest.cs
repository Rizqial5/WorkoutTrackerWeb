namespace WorkoutTracker.Backend.Models
{
    public class WorkoutPlansPostRequest
    {
        public string PlansName { get; set; }
        public ICollection<int> ExercisesSetExercisesCollection { get; set; }

    }
}
