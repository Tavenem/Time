using System.Text.Json;
using System.Text.Json.Serialization;

namespace Tavenem.Time.Converters;

/// <summary>
/// Converts a <see cref="CosmicTime"/> to or from JSON.
/// </summary>
public class CosmicTimeConverter : JsonConverter<CosmicTime>
{
    /// <summary>Reads and converts the JSON to type <see cref="CosmicTime"/>.</summary>
    /// <param name="reader">The reader.</param>
    /// <param name="typeToConvert">The type to convert.</param>
    /// <param name="options">An object that specifies serialization options to use.</param>
    /// <returns>The converted value.</returns>
    public override CosmicTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType != JsonTokenType.StartObject
            || !reader.Read()
            || reader.TokenType != JsonTokenType.PropertyName)
        {
            throw new JsonException();
        }

        var prop = reader.GetString();
        if (!string.Equals(
            prop,
            nameof(CosmicTime.CurrentEpoch),
            options.PropertyNameCaseInsensitive
                ? StringComparison.OrdinalIgnoreCase
                : StringComparison.Ordinal))
        {
            throw new JsonException();
        }
        if (!reader.Read()
            || reader.TokenType != JsonTokenType.String)
        {
            throw new JsonException();
        }
        var currentEpoch = reader.GetString();

        if (!reader.Read()
            || reader.TokenType != JsonTokenType.PropertyName)
        {
            throw new JsonException();
        }
        prop = reader.GetString();
        if (!string.Equals(
            prop,
            nameof(CosmicTime.Epochs),
            options.PropertyNameCaseInsensitive
                ? StringComparison.OrdinalIgnoreCase
                : StringComparison.Ordinal))
        {
            throw new JsonException();
        }
        if (!reader.Read())
        {
            throw new JsonException();
        }
        var epochs = JsonSerializer.Deserialize<List<Epoch>>(ref reader, options);

        if (!reader.Read()
            || reader.TokenType != JsonTokenType.PropertyName)
        {
            throw new JsonException();
        }
        prop = reader.GetString();
        if (!string.Equals(
            prop,
            nameof(CosmicTime.Now),
            options.PropertyNameCaseInsensitive
                ? StringComparison.OrdinalIgnoreCase
                : StringComparison.Ordinal))
        {
            throw new JsonException();
        }
        if (!reader.Read()
            || reader.TokenType != JsonTokenType.StartObject)
        {
            throw new JsonException();
        }
        var now = JsonSerializer.Deserialize<Instant>(ref reader, options);
        if (reader.TokenType != JsonTokenType.EndObject)
        {
            throw new JsonException();
        }
        reader.Read();

        while (reader.TokenType != JsonTokenType.EndObject)
        {
            reader.Read();
        }

        return new CosmicTime(now, epochs, currentEpoch);
    }

    /// <summary>Writes a specified value as JSON.</summary>
    /// <param name="writer">The writer to write to.</param>
    /// <param name="value">The value to convert to JSON.</param>
    /// <param name="options">An object that specifies serialization options to use.</param>
    public override void Write(Utf8JsonWriter writer, CosmicTime value, JsonSerializerOptions options)
    {
        writer.WriteStartObject();

        writer.WriteString(
            options.PropertyNamingPolicy is null
                ? nameof(CosmicTime.CurrentEpoch)
                : options.PropertyNamingPolicy.ConvertName(nameof(CosmicTime.CurrentEpoch)),
            value.CurrentEpoch);

        if (value.Epochs.Count > 0)
        {
            writer.WritePropertyName(options.PropertyNamingPolicy is null
                ? nameof(CosmicTime.Epochs)
                : options.PropertyNamingPolicy.ConvertName(nameof(CosmicTime.Epochs)));
            JsonSerializer.Serialize(writer, value.Epochs, options);
        }
        else
        {
            writer.WriteNull(options.PropertyNamingPolicy is null
                ? nameof(CosmicTime.Epochs)
                : options.PropertyNamingPolicy.ConvertName(nameof(CosmicTime.Epochs)));
        }

        writer.WritePropertyName(options.PropertyNamingPolicy is null
            ? nameof(CosmicTime.Now)
            : options.PropertyNamingPolicy.ConvertName(nameof(CosmicTime.Now)));
        JsonSerializer.Serialize(writer, value.Now, options);

        writer.WriteEndObject();
    }
}
