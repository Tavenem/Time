using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Numerics;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Tavenem.Time.Converters;

/// <summary>
/// Converts a <see cref="Duration"/> to or from JSON.
/// </summary>
public class RelativeDurationConverter : JsonConverter<RelativeDuration>
{
    /// <summary>Determines whether the specified type can be converted.</summary>
    /// <param name="typeToConvert">The type to compare against.</param>
    /// <returns>
    /// <see langword="true" /> if the type can be converted; otherwise, <see langword="false" />.
    /// </returns>
    public override bool CanConvert(Type typeToConvert) => typeToConvert == typeof(RelativeDuration);

    /// <summary>Reads and converts the JSON to <see cref="BigInteger"/>.</summary>
    /// <param name="reader">The reader.</param>
    /// <param name="typeToConvert">The type to convert.</param>
    /// <param name="options">An object that specifies serialization options to use.</param>
    /// <returns>The converted value.</returns>
    public override RelativeDuration Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) => RelativeDuration.ParseExact(
        reader.GetString(),
        "o",
        CultureInfo.InvariantCulture);

    /// <inheritdoc />
    public override RelativeDuration ReadAsPropertyName(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        => Read(ref reader, typeToConvert, options);

    /// <summary>Writes a specified value as JSON.</summary>
    /// <param name="writer">The writer to write to.</param>
    /// <param name="value">The value to convert to JSON.</param>
    /// <param name="options">An object that specifies serialization options to use.</param>
    public override void Write(Utf8JsonWriter writer, RelativeDuration value, JsonSerializerOptions options)
        => writer.WriteStringValue(value.ToString("o", CultureInfo.InvariantCulture));

    /// <inheritdoc />
    public override void WriteAsPropertyName(Utf8JsonWriter writer, [DisallowNull] RelativeDuration value, JsonSerializerOptions options)
        => writer.WritePropertyName(value.ToString("o", CultureInfo.InvariantCulture));
}
