using HealthTracker.Modules.Profiles.Features;
using HealthTracker.SharedKernel.Modules;
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

    public void AddServices(IServiceCollection services, IConfiguration configuration)
    {
        // Persistence and services arrive with #4 (EF Core) and #7 (households & profiles).
    }

    public void MapEndpoints(IEndpointRouteBuilder endpoints)
    {
        var group = endpoints.MapGroup("/profiles").WithTags(Name);

        GetProfile.Map(group);
    }
}
