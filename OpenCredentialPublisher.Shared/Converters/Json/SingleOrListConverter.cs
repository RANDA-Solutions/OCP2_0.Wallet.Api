using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace OpenCredentialPublisher.Shared.Converters.Json
{
        public class SingleOrListConverter<T> : JsonConverter<List<T>> where T : class
        {
            public override bool CanConvert(System.Type objectType) => objectType == typeof(List<T>);

            public override List<T> Read(
                ref Utf8JsonReader reader,
                System.Type typeToConvert,
                JsonSerializerOptions options)
            {
                var objList = new List<T>();
                using JsonDocument jsonDocument = JsonDocument.ParseValue(ref reader);
                if (jsonDocument.RootElement.ValueKind == JsonValueKind.Array)
                {
                    foreach (JsonElement enumerate in jsonDocument.RootElement.EnumerateArray())
                        objList.Add(JsonSerializer.Deserialize<T>(enumerate.GetRawText(), options));
                }
                else if (jsonDocument.RootElement.ValueKind == JsonValueKind.Object)
                    objList.Add(JsonSerializer.Deserialize<T>(jsonDocument.RootElement.GetRawText(), options));

                return objList;
            }

            public override void Write(Utf8JsonWriter writer, List<T> value, JsonSerializerOptions options)
            {
                if (value is { Count: 1 })
                {
                    JsonDocument.Parse(JsonSerializer.Serialize(value[0], options)).WriteTo(writer);
                }
                else
                {
                    if (value is not { Count: > 1 })
                        return;
                    writer.WriteStartArray();
                    foreach (T obj in value)
                        JsonDocument.Parse(JsonSerializer.Serialize(obj, options)).WriteTo(writer);
                    writer.WriteEndArray();
                }
            }
        }

}
