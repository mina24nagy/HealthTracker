using Microsoft.EntityFrameworkCore;

namespace HealthTracker.Modules.Profiles.Persistence;

/// <summary>
/// The Profiles module's own database context. All its tables live in the <c>profiles</c> schema,
/// and no other module may read or write them (ADR-0002, ADR-0004). Entities arrive with #7.
/// </summary>
internal sealed class ProfilesDbContext(DbContextOptions<ProfilesDbContext> options) : DbContext(options)
{
    public const string Schema = "profiles";

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema(Schema);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ProfilesDbContext).Assembly);
    }
}
