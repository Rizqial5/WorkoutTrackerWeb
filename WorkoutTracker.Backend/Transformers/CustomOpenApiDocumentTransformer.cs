using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.OpenApi;

public class CustomOpenApiDocumentTransformer : IOpenApiDocumentTransformer
{
    public Task TransformAsync(OpenApiDocument document, OpenApiDocumentTransformerContext context, CancellationToken cancellationToken)
    {

        // Tambahkan tag menggunakan fungsi helper
        AddTagDescription(document, "Auth", "Endpoint terkait produk di aplikasi ini.");
        AddTagDescription(document, "ExerciseSets", "Endpoint terkait pengguna di aplikasi ini.");
        AddTagDescription(document, "WorkoutPlans", "Endpoint terkait pesanan di aplikasi ini.");

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
