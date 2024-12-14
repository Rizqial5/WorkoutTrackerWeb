using WorkoutTracker.Backend.Models;

namespace WorkoutTracker.Backend.Utilities
{
    public class ResponsesHelper
    {
        public static SchedulePlansResponse SchedulePlansResponse(SchedulePlans schedulePlans)
        {
            var response = new SchedulePlansResponse
            {
                Id = schedulePlans.Id,
                PlannedDateTime = schedulePlans.ScheduleTime,
                WorkoutPlanResponse = new WorkoutPlanResponse
                {
                    PlanId = schedulePlans.WorkoutPlansId,
                    PlanName = schedulePlans.WorkoutPlan.PlanName,
                    ExerciseSets = null,
                    ScheduledTime = null

                }
            };
            return response;
        }

        public static ExerciseSetResponse ExerciseSetResponse(ExerciseSet exerciseSet)
        {
            var response = new ExerciseSetResponse
            {
                ExerciseSetId = exerciseSet.ExerciseSetId,
                ExerciseSetName = exerciseSet.ExerciseSetName,
                Exercise = new ExerciseDataResponse
                {
                    Name = exerciseSet!.Exercise!.Name!,
                    CategoryWorkout = exerciseSet.Exercise.CategoryWorkout,
                    MuscleGroup = exerciseSet.Exercise.MuscleGroup

                },
                Set = exerciseSet.Set,
                Repetitions = exerciseSet.Repetitions,
                Weight = exerciseSet.Weight

            };
            return response;
        }
    }
}
