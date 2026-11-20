using Microsoft.AspNetCore.Diagnostics.HealthChecks;

namespace HealthTracker.Api.Infrastructure;

/// <summary>
/// Liveness: is the process up? (runs no checks, so a slow database never restarts the app)
/// Readiness: can it serve traffic? (runs checks tagged <see cref="ReadyTag"/>, e.g. PostgreSQL from #4)
/// </summary>
internal static class HealthEndpoints
{
    public const string ReadyTag = "ready";

    public static WebApplication MapHealthEndpoints(this WebApplication app)
    {
        app.MapHealthChecks("/health/live", new HealthCheckOptions { Predicate = _ => false })
            .AllowAnonymous();

        app.MapHealthChecks("/health/ready", new HealthCheckOptions { Predicate = check => check.Tags.Contains(ReadyTag) })
            .AllowAnonymous();

        return app;
    }
}
