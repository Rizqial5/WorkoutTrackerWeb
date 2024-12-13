namespace WorkoutTracker.Backend.Models
{
    public class CreateExerciseSetRequest
    {
        public string ExerciseSetName { get; set; }
        public int Set { get; set; }

        public int Repetitions { get; set; }
        public int ExerciseId { get; set; }
    }
}
