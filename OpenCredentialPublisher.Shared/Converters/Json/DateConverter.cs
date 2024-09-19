using System;
using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace OpenCredentialPublisher.Shared.Converters.Json
{
    public class DateConverter : JsonConverter<DateTime>
    {
        private const string Format = "o";

        public override DateTime Read(
            ref Utf8JsonReader reader,
            System.Type typeToConvert,
            JsonSerializerOptions options)
        {
            return DateTime.Parse(reader.GetString(), (IFormatProvider)CultureInfo.InvariantCulture);
        }

        public override void Write(
            Utf8JsonWriter writer,
            DateTime dateTimeValue,
            JsonSerializerOptions options)
        {
            writer.WriteStringValue(dateTimeValue.Kind == DateTimeKind.Utc ? dateTimeValue.ToString("o") : dateTimeValue.ToUniversalTime().ToString("o"));
        }
    }
}
