using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WorkoutTracker.Backend.Models
{
    public class WorkoutPlans
    {
        [Key]
        public int PlanId { get; set; }

        [Required]
        [StringLength(50)]
        public string PlanName { get; set; }
        
        public ICollection<SchedulePlans> ScheduledTime { get; set; }

        
        public ICollection<ExerciseSet> ExerciseSets { get; set; }

        [ForeignKey("UserId")]
        public string UserId{ get; set; }
        public IdentityUser User { get; set; }

    }

}
