﻿using System.ComponentModel.DataAnnotations;

namespace WorkoutTracker.Backend.Models
{
    public class WorkoutPlans
    {
        [Key]
        public int PlanId { get; set; }

        [Required]
        [StringLength(50)]
        public string PlanName { get; set; }

        public ICollection<ExerciseData> Exercises { get; set; }

    }
}
