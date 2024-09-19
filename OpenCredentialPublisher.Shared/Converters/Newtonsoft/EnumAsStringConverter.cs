using System;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace OpenCredentialPublisher.Shared.Converters.Newtonsoft
{
    public class EnumAsStringConverter<T> : JsonConverter where T : Enum
    {
        public override bool CanConvert(Type objectType) => objectType.IsEnum;

        public override object ReadJson(
            JsonReader reader,
            Type objectType,
            object existingValue,
            JsonSerializer serializer)
        {
            var jToken = JToken.Load(reader);
            if (typeof(T).IsEnum && jToken.Type == JTokenType.String)
            {
                if (Enum.TryParse(typeof(T), jToken.Value<string>(), out var result))
                    return result;
            }
            return "InvalidValue";
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            if (value == null)
            {
                writer.WriteNull();
            }
            else
            {
                var names = Enum.GetNames(typeof(T));
                var str = value.ToString();
                if (!names.Contains(str))
                    return;
                serializer.Serialize(writer, str);
            }
        }
    }
}
