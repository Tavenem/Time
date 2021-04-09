using System;
using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Tavenem.Time
{
    /// <summary>
    /// Converts a <see cref="Duration"/> to or from JSON.
    /// </summary>
    public class DurationConverter : JsonConverter<Duration>
    {
        /// <summary>Reads and converts the JSON to type <see cref="Duration"/>.</summary>
        /// <param name="reader">The reader.</param>
        /// <param name="typeToConvert">The type to convert.</param>
        /// <param name="options">An object that specifies serialization options to use.</param>
        /// <returns>The converted value.</returns>
        public override Duration Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            => Duration.ParseExact(reader.GetString(), "o", CultureInfo.InvariantCulture);

        /// <summary>Writes a specified value as JSON.</summary>
        /// <param name="writer">The writer to write to.</param>
        /// <param name="value">The value to convert to JSON.</param>
        /// <param name="options">An object that specifies serialization options to use.</param>
        public override void Write(Utf8JsonWriter writer, Duration value, JsonSerializerOptions options)
            => writer.WriteStringValue(value.ToString("o", CultureInfo.InvariantCulture));
    }
}
