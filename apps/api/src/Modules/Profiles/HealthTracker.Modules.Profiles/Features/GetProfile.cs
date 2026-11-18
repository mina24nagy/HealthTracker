using HealthTracker.SharedKernel;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Routing;

namespace HealthTracker.Modules.Profiles.Features;

/// <summary>
/// Vertical slice: GET /api/profiles/{profileId}.
/// Placeholder until #7 adds persistence: it always answers 404, but already shows the slice shape,
/// <see cref="ProfileId"/> route binding and the ProblemDetails error format.
/// </summary>
internal static class GetProfile
{
    internal sealed record ProfileResponse(ProfileId Id, string DisplayName);

    public static void Map(IEndpointRouteBuilder group) =>
        group.MapGet("/{profileId}", Handle)
            .WithName("GetProfile")
            .WithSummary("Gets a profile by id.")
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound);

    private static Results<Ok<ProfileResponse>, NotFound> Handle(ProfileId profileId) => TypedResults.NotFound();
}
