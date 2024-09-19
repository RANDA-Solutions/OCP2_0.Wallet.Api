using System;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace OpenCredentialPublisher.Shared.Converters.Newtonsoft
{
    public class StringOrTypeConverter<T> : JsonConverter where T : class, new()
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType.GetProperties().Any(prop => prop.Name == "Id");
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            JToken token = JToken.Load(reader);
            dynamic obj = new T();
            if (token.Type == JTokenType.String)
            {
                obj.Id = token.Value<string>();
            }
            else if (token.Type == JTokenType.Object)
            {
                obj = token.ToObject<T>();
            }
            return obj;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            serializer.Serialize(writer, value);
        }
    }
}
