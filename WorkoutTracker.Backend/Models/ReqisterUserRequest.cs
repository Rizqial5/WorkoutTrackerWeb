namespace WorkoutTracker.Backend.Models
{
    public class ReqisterUserRequest
    {
        public string? Email { get; set; }
        public string? Username { get; set; }
        public string? Password { get; set; }
    }
}
