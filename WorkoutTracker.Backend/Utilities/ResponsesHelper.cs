using Microsoft.AspNetCore.Mvc;
using WorkoutTracker.Backend.Models;

namespace WorkoutTracker.Backend.Utilities
{
    public class ResponsesHelper
    {

        public static WorkoutPlanResponse WorkoutPlanResponse([FromBody]WorkoutPlans workoutPlan)
        {
            

            var response = new WorkoutPlanResponse
            {
                PlanName = workoutPlan.PlanName,
                PlanId = workoutPlan.PlanId,
                User = workoutPlan.User.UserName,
                ExerciseSets = workoutPlan.ExerciseSets.Select(ExerciseSetResponse).ToList(),
            };

            if (workoutPlan.ScheduledTime != null)
            {
                response.ScheduledTime = workoutPlan.ScheduledTime.Select(SchedulePlansResponse).ToList();
            }
            else
            {
                response.ScheduledTime = null;
            }


            return response;
        }

        public static SchedulePlansResponse SchedulePlansResponse([FromBody] SchedulePlans schedulePlans)
        {
            if (schedulePlans == null) return null;

            var response = new SchedulePlansResponse
            {
                Id = schedulePlans.Id,
                PlannedDateTime = schedulePlans.ScheduleTime,
                User = schedulePlans.User.UserName,
                PlanStatus = schedulePlans.PlanStatus,
                PlanId = schedulePlans.WorkoutPlan.PlanId,
                PlanName = schedulePlans.WorkoutPlan.PlanName
            };
            return response;
        }

        public static ExerciseSetResponse ExerciseSetResponse(ExerciseSet exerciseSet)
        {
            var response = new ExerciseSetResponse
            {
                ExerciseSetId = exerciseSet.ExerciseSetId,
                User = exerciseSet.User.UserName,
                ExerciseSetName = exerciseSet.ExerciseSetName,
                Exercise = ExerciseDataResponse(exerciseSet.Exercise!),
                Set = exerciseSet.Set,
                Repetitions = exerciseSet.Repetitions,
                Weight = exerciseSet.Weight

            };
            return response;
        }

        public static ExerciseDataResponse ExerciseDataResponse(ExerciseData exerciseData)
        {
            var response = new ExerciseDataResponse
            {
                Id = exerciseData.ExerciseId,
                Name = exerciseData.Name,
                CategoryWorkout = exerciseData.CategoryWorkout,
                MuscleGroup = exerciseData.MuscleGroup
            };

            return response;
        }

        
    }
}
