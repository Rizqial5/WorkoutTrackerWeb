using WorkoutTracker.Backend.Models;

namespace WorkoutTracker.Backend.Utilities
{
    public class ResponsesHelper
    {

        public static WorkoutPlanResponse WorkoutPlanResponse(WorkoutPlans workoutPlan)
        {
            

            var response = new WorkoutPlanResponse
            {
                PlanName = workoutPlan.PlanName,
                PlanId = workoutPlan.PlanId,
                ExerciseSets = workoutPlan.ExerciseSets.Select(ExerciseSetResponse).ToList(),
                PlanStatus = workoutPlan.PlanStatus
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

        public static SchedulePlansResponse SchedulePlansResponse(SchedulePlans schedulePlans)
        {
            if (schedulePlans == null) return null;

            var response = new SchedulePlansResponse
            {
                Id = schedulePlans.Id,
                PlannedDateTime = schedulePlans.ScheduleTime,
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
                Name = exerciseData.Name,
                CategoryWorkout = exerciseData.CategoryWorkout,
                MuscleGroup = exerciseData.MuscleGroup
            };

            return response;
        }
    }
}
