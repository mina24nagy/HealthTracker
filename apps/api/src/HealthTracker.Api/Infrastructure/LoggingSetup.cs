namespace HealthTracker.Api.Infrastructure;

internal static class LoggingSetup
{
    /// <summary>
    /// Human-readable console logs in Development; one JSON object per line elsewhere,
    /// so a log collector can parse them (OpenTelemetry export comes with the first deploy, M6).
    /// </summary>
    public static WebApplicationBuilder AddStructuredLogging(this WebApplicationBuilder builder)
    {
        if (!builder.Environment.IsDevelopment())
        {
            builder.Logging.ClearProviders();
            builder.Logging.AddJsonConsole(options =>
            {
                options.IncludeScopes = true;
                options.UseUtcTimestamp = true;
                options.TimestampFormat = "yyyy-MM-ddTHH:mm:ss.fffZ ";
            });
        }

        return builder;
    }
}
