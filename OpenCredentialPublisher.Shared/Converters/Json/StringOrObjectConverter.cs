using System.Text.Json;
using System.Text.Json.Serialization;

namespace OpenCredentialPublisher.Shared.Converters.Json
{
    public class StringOrObjectConverter<T> : JsonConverter<object> where T : class
    {
        public override bool CanConvert(System.Type objectType)
        {
            return objectType == typeof(object) || objectType == typeof(T) || objectType == typeof(string);
        }

        public override object Read(
            ref Utf8JsonReader reader,
            System.Type typeToConvert,
            JsonSerializerOptions options)
        {
            using JsonDocument jsonDocument = JsonDocument.ParseValue(ref reader);
            JsonElement rootElement = jsonDocument.RootElement;
            if (rootElement.ValueKind == JsonValueKind.Object)
            {
                rootElement = jsonDocument.RootElement;
                return (object)JsonSerializer.Deserialize<T>(rootElement.GetRawText(), options);
            }
            rootElement = jsonDocument.RootElement;
            return (object)rootElement.ToString();
        }

        public override void Write(Utf8JsonWriter writer, object value, JsonSerializerOptions options)
        {
            if (value is T obj)
                JsonDocument.Parse(JsonSerializer.Serialize<T>(obj, options)).WriteTo(writer);
            else
                writer.WriteStringValue(value.ToString());
        }
    }

}
