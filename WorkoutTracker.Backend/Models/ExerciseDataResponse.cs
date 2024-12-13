namespace WorkoutTracker.Backend.Models
{
    public class ExerciseDataResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public CategoryWorkout CategoryWorkout { get; set; }
        public MuscleGroup MuscleGroup { get; set; }
    }
}
