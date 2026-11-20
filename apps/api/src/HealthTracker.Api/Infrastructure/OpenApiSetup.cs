using HealthTracker.SharedKernel;
using Microsoft.OpenApi;
using Scalar.AspNetCore;

namespace HealthTracker.Api.Infrastructure;

/// <summary>
/// OpenAPI document (the contract the TypeScript client is generated from, ADR-0005) and the Scalar UI.
/// </summary>
internal static class OpenApiSetup
{
    public static IServiceCollection AddOpenApiDocumentation(this IServiceCollection services) =>
        services.AddOpenApi(options =>
        {
            options.AddDocumentTransformer((document, _, _) =>
            {
                document.Info.Title = "HealthTracker API";
                document.Info.Description = "Personal & family health record.";
                return Task.CompletedTask;
            });

            // Strongly typed ids have a custom JSON converter, so describe them explicitly as UUID strings.
            options.AddSchemaTransformer((schema, context, _) =>
            {
                if (context.JsonTypeInfo.Type == typeof(ProfileId))
                {
                    schema.Type = JsonSchemaType.String;
                    schema.Format = "uuid";
                    schema.Properties?.Clear();
                }

                return Task.CompletedTask;
            });

            // Route/query parameters bind via TryParse, so their schema needs the same hint.
            options.AddOperationTransformer((operation, context, _) =>
            {
                var idParameters = context.Description.ParameterDescriptions
                    .Where(p => p.Type == typeof(ProfileId))
                    .Select(p => p.Name)
                    .ToHashSet(StringComparer.OrdinalIgnoreCase);

                foreach (var parameter in operation.Parameters ?? [])
                {
                    if (parameter.Name is not null && idParameters.Contains(parameter.Name) && parameter.Schema is OpenApiSchema schema)
                    {
                        schema.Format = "uuid";
                    }
                }

                return Task.CompletedTask;
            });
        });

    /// <summary>Development only: production does not expose the API description.</summary>
    public static WebApplication MapOpenApiDocumentation(this WebApplication app)
    {
        if (app.Environment.IsDevelopment())
        {
            app.MapOpenApi();                 // /openapi/v1.json
            app.MapScalarApiReference();      // /scalar
        }

        return app;
    }
}
