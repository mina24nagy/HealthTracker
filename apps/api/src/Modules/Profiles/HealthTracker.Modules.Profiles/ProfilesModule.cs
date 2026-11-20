using HealthTracker.Modules.Profiles.Features;
using HealthTracker.Modules.Profiles.Persistence;
using HealthTracker.SharedKernel.Modules;
using HealthTracker.SharedKernel.Persistence;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace HealthTracker.Modules.Profiles;

/// <summary>Households, family profiles and access roles (ADR-0006). The only public type of this module.</summary>
public sealed class ProfilesModule : IModule
{
    public string Name => "Profiles";

    public void AddServices(IServiceCollection services, IConfiguration configuration) =>
        services.AddModuleDbContext<ProfilesDbContext>(ProfilesDbContext.Schema);

    public void MapEndpoints(IEndpointRouteBuilder endpoints)
    {
        var group = endpoints.MapGroup("/profiles").WithTags(Name);

        GetProfile.Map(group);
    }
}
