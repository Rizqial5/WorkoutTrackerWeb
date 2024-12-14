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
    }
}
