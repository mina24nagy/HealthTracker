using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace HealthTracker.SharedKernel;

/// <summary>
/// Identifies a person whose health is recorded. Every health record belongs to exactly one profile (ADR-0006).
/// A strongly typed id prevents mixing it up with other Guid ids at compile time.
/// </summary>
/// <remarks>
/// Crosses boundaries as a plain UUID string: <see cref="IParsable{TSelf}"/> lets Minimal APIs bind it from
/// routes and query strings, and <see cref="ProfileIdJsonConverter"/> (de)serializes it in JSON bodies.
/// </remarks>
[JsonConverter(typeof(ProfileIdJsonConverter))]
public readonly record struct ProfileId(Guid Value) : IParsable<ProfileId>
{
    /// <summary>Creates a new time-ordered (UUIDv7) id.</summary>
    public static ProfileId New() => new(Guid.CreateVersion7());

    public static ProfileId Parse(string s, IFormatProvider? provider) =>
        TryParse(s, provider, out var id) ? id : throw new FormatException($"'{s}' is not a valid profile id.");

    /// <summary>Parses a UUID string. <see cref="Guid.Empty"/> is rejected: it is never a real profile.</summary>
    public static bool TryParse([NotNullWhen(true)] string? s, IFormatProvider? provider, out ProfileId result)
    {
        if (Guid.TryParse(s, provider, out var guid) && guid != Guid.Empty)
        {
            result = new ProfileId(guid);
            return true;
        }

        result = default;
        return false;
    }

    public override string ToString() => Value.ToString();
}

/// <summary>Serializes <see cref="ProfileId"/> as <c>"0193…"</c> instead of <c>{ "value": "0193…" }</c>.</summary>
internal sealed class ProfileIdJsonConverter : JsonConverter<ProfileId>
{
    public override ProfileId Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType == JsonTokenType.String && reader.TryGetGuid(out var guid) && guid != Guid.Empty)
        {
            return new ProfileId(guid);
        }

        throw new JsonException("A profile id must be a non-empty UUID string.");
    }

    public override void Write(Utf8JsonWriter writer, ProfileId value, JsonSerializerOptions options) =>
        writer.WriteStringValue(value.Value);
}
