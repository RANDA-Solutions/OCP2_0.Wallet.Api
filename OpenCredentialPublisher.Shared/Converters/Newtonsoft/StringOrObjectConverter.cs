using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace OpenCredentialPublisher.Shared.Converters.Newtonsoft
{
    public class StringOrObjectConverter<T> : JsonConverter
    {
        public override bool CanConvert(System.Type objectType)
        {
            return objectType == typeof(object) || objectType == typeof(T) || objectType == typeof(string);
        }

        public override object ReadJson(
            JsonReader reader,
            System.Type objectType,
            object existingValue,
            JsonSerializer serializer)
        {
            JToken jtoken = JToken.Load(reader);
            return jtoken.Type == JTokenType.Object ? (object)serializer.Deserialize<T>(jtoken.CreateReader()) : (object)jtoken.Value<string>();
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            serializer.Serialize(writer, value);
        }
    }
}
