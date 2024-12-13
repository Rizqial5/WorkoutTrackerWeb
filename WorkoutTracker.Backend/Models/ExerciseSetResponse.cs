namespace WorkoutTracker.Backend.Models
{
    public class ExerciseSetResponse
    {
        public int ExerciseSetId { get; set; }
        public string ExerciseSetName { get; set; }
        public int Repetitions { get; set; }
        public int Set { get; set; }
        public string Weight { get; set; }
        public ExerciseDataResponse Exercise { get; set; }
    }
}
