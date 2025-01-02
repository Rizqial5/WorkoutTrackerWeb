using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.OpenApi;

public class CustomOpenApiDocumentTransformer : IOpenApiDocumentTransformer
{
    public Task TransformAsync(OpenApiDocument document, OpenApiDocumentTransformerContext context, CancellationToken cancellationToken)
    {

        // Tambahkan tag menggunakan fungsi helper
        AddTagDescription(document, "Auth", "Using JWT Authentication for Login and Register a new Account");
        AddTagDescription(
            document, 
            "ExerciseDatas", 
            "Exercise Data contains a list of all workout movements input by the admin, which users can later add to their exercise sets when creating a workout plan."
        );

        AddTagDescription(
            document, 
            "ExerciseSets", 
            "The ExerciseSets endpoint allows users to create their own exercises, specifying details such as the number of repetitions, weight used, and the number of sets based on the existing exercises."
        );
        AddTagDescription(
            document, 
            "WorkoutPlans", 
            "The WorkoutPlans endpoint enables users to create their own workout plans, consisting of multiple pre-defined exercise sets. These plans can be tracked for progress, and users can also schedule their workouts efficiently."
        );
        AddTagDescription(
            document, 
            "WorkoutPlans", 
            "The WorkoutPlans endpoint enables users to create their own workout plans, consisting of multiple pre-defined exercise sets. These plans can be tracked for progress, and users can also schedule their workouts efficiently."
        );
        AddTagDescription(
            document, 
            "SchedulePlans", 
            "The SchedulePlans endpoint allows users to assign schedules to their previously created workout plans. Additionally, users can mark workout plans as done once they have been completed."
        );
        
        return Task.CompletedTask;
    }

    private void AddTagDescription(OpenApiDocument document, string nameTag, string description)
    {
       
        foreach (var item in document.Tags)
        {
            if(item.Name == nameTag)
            {
                item.Description = description;
            }
        }

        
        
    }
}
