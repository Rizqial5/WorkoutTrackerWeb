using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace WorkoutTracker.Backend.Models
{
    public class SchedulePlans
    {
        [Key]
        public int Id { get; set; }

        public DateTime ScheduleTime { get; set; }

        public PlanStatus PlanStatus { get; set; }

        public int WorkoutPlansId { get; set; }
        //Navigation Property
        [JsonIgnore]
        public WorkoutPlans WorkoutPlan { get; set; }

        [ForeignKey("UserId")]
        public string UserId { get; set; }
        public IdentityUser User { get; set; }
    }

    public enum PlanStatus
    {
        Active,
        Done,
    }
}
