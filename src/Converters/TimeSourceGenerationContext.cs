using System.Text.Json.Serialization;

namespace Tavenem.Time;

/// <summary>
/// A <see cref="JsonSerializerContext"/> for <c>Tavenem.Time</c>
/// </summary>
[JsonSerializable(typeof(Duration))]
[JsonSerializable(typeof(RelativeDuration))]
[JsonSerializable(typeof(Epoch))]
[JsonSerializable(typeof(Instant))]
[JsonSerializable(typeof(CosmicTime))]
public partial class TimeSourceGenerationContext
    : JsonSerializerContext
{ }
