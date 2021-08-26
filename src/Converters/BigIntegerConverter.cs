using System.Globalization;
using System.Numerics;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Tavenem.Time.Converters;

/// <summary>
/// Converts a <see cref="BigInteger"/> to or from JSON.
/// </summary>
public class BigIntegerConverter : JsonConverter<BigInteger>
{
    /// <summary>Determines whether the specified type can be converted.</summary>
    /// <param name="typeToConvert">The type to compare against.</param>
    /// <returns>
    /// <see langword="true" /> if the type can be converted; otherwise, <see langword="false" />.
    /// </returns>
    public override bool CanConvert(Type typeToConvert) => typeToConvert == typeof(BigInteger);

    /// <summary>Reads and converts the JSON to <see cref="BigInteger"/>.</summary>
    /// <param name="reader">The reader.</param>
    /// <param name="typeToConvert">The type to convert.</param>
    /// <param name="options">An object that specifies serialization options to use.</param>
    /// <returns>The converted value.</returns>
    public override BigInteger Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) => BigInteger.TryParse(
        reader.GetString(),
        NumberStyles.Integer,
        CultureInfo.InvariantCulture,
        out var result)
        ? result
        : throw new JsonException();

    /// <summary>Writes a specified value as JSON.</summary>
    /// <param name="writer">The writer to write to.</param>
    /// <param name="value">The value to convert to JSON.</param>
    /// <param name="options">An object that specifies serialization options to use.</param>
    public override void Write(Utf8JsonWriter writer, BigInteger value, JsonSerializerOptions options)
        => writer.WriteRawValue($"\"{value.ToString(CultureInfo.InvariantCulture)}\"", skipInputValidation: true);
}
