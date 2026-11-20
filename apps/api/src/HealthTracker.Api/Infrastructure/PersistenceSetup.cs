using HealthTracker.SharedKernel.Persistence;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Npgsql;

namespace HealthTracker.Api.Infrastructure;

/// <summary>
/// The host owns the single PostgreSQL connection pool; every module's DbContext uses it (ADR-0004).
/// </summary>
internal static partial class PersistenceSetup
{
    public static WebApplicationBuilder AddPostgres(this WebApplicationBuilder builder)
    {
        var connectionString = builder.Configuration.GetConnectionString(ModulePersistence.ConnectionStringName)
            ?? throw new InvalidOperationException(
                $"Connection string '{ModulePersistence.ConnectionStringName}' is missing. " +
                "Set ConnectionStrings__HealthTracker (see .env.example and docs).");

        builder.Services.AddSingleton(_ => NpgsqlDataSource.Create(connectionString));

        builder.Services.AddHealthChecks()
            .AddCheck<PostgresHealthCheck>("postgres", tags: [HealthEndpoints.ReadyTag], timeout: TimeSpan.FromSeconds(3));

        return builder;
    }

    /// <summary>
    /// Development convenience: bring every module's schema up to date at startup.
    /// Other environments apply migrations as an explicit deployment step (decided in M6).
    /// </summary>
    public static async Task MigrateModuleDatabasesAsync(this WebApplication app)
    {
        if (!app.Environment.IsDevelopment())
        {
            return;
        }

        await using var scope = app.Services.CreateAsyncScope();
        var logger = scope.ServiceProvider.GetRequiredService<ILoggerFactory>().CreateLogger("Migrations");

        foreach (var database in scope.ServiceProvider.GetServices<IModuleDatabase>())
        {
            LogMigrating(logger, database.Name);
            await database.MigrateAsync(app.Lifetime.ApplicationStopping);
        }
    }

    [LoggerMessage(Level = LogLevel.Information, Message = "Applying migrations for {Database}")]
    private static partial void LogMigrating(ILogger logger, string database);
}

/// <summary>Readiness: can we run a trivial query against PostgreSQL?</summary>
internal sealed class PostgresHealthCheck(NpgsqlDataSource dataSource) : IHealthCheck
{
    public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
    {
        try
        {
            await using var command = dataSource.CreateCommand("SELECT 1");
            await command.ExecuteScalarAsync(cancellationToken);
            return HealthCheckResult.Healthy();
        }
        catch (NpgsqlException exception)
        {
            return HealthCheckResult.Unhealthy("PostgreSQL is unreachable.", exception);
        }
    }
}
