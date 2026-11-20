using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace HealthTracker.SharedKernel.Modules;

/// <summary>
/// Entry point of a module in the modular monolith (ADR-0002).
/// The host lists modules explicitly and calls these two hooks; everything else in a module stays internal.
/// </summary>
public interface IModule
{
    /// <summary>Human-readable module name, used in logs and OpenAPI tags.</summary>
    string Name { get; }

    /// <summary>Registers the module's services in the shared container.</summary>
    void AddServices(IServiceCollection services, IConfiguration configuration);

    /// <summary>Maps the module's endpoints under the host's <c>/api</c> route group.</summary>
    void MapEndpoints(IEndpointRouteBuilder endpoints);
}
