using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;

namespace HealthTracker.SharedKernel.Persistence;

/// <summary>
/// Persistence building block for modules (ADR-0004): one PostgreSQL database, one schema per module.
/// </summary>
public static class ModulePersistence
{
    /// <summary>Name of the single connection string, i.e. <c>ConnectionStrings:HealthTracker</c>.</summary>
    public const string ConnectionStringName = "HealthTracker";

    /// <summary>
    /// Registers a module's <typeparamref name="TContext"/> on the host's shared <see cref="NpgsqlDataSource"/>,
    /// with its migrations history table inside the module's own <paramref name="schema"/>.
    /// The context itself must call <c>HasDefaultSchema(schema)</c> so its tables land in the same schema.
    /// </summary>
    public static IServiceCollection AddModuleDbContext<TContext>(this IServiceCollection services, string schema)
        where TContext : DbContext
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(schema);

        services.AddDbContext<TContext>((provider, options) =>
            options.UseNpgsql(
                provider.GetRequiredService<NpgsqlDataSource>(),
                npgsql => npgsql.MigrationsHistoryTable(HistoryRepository.DefaultTableName, schema)));

        services.AddScoped<IModuleDatabase, ModuleDatabase<TContext>>();

        return services;
    }
}

/// <summary>A module's database, as seen by the host (e.g. to apply migrations at startup in Development).</summary>
public interface IModuleDatabase
{
    string Name { get; }

    Task MigrateAsync(CancellationToken cancellationToken);
}

internal sealed class ModuleDatabase<TContext>(TContext context) : IModuleDatabase
    where TContext : DbContext
{
    public string Name => typeof(TContext).Name;

    public Task MigrateAsync(CancellationToken cancellationToken) => context.Database.MigrateAsync(cancellationToken);
}
