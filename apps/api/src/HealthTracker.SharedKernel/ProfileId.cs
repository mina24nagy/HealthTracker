namespace HealthTracker.SharedKernel;

/// <summary>
/// Identifies a person whose health is recorded. Every health record belongs to exactly one profile (ADR-0006).
/// A strongly typed id prevents mixing it up with other Guid ids at compile time.
/// </summary>
public readonly record struct ProfileId(Guid Value)
{
    public static ProfileId New() => new(Guid.CreateVersion7());

    public override string ToString() => Value.ToString();
}
