using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace OpenCredentialPublisher.Shared.Converters.Newtonsoft
{
    public class DateConverter<T> : JsonConverter
    {
        public string Format { get; set; }

        public DateConverter(string format) => this.Format = format;

        public override bool CanConvert(System.Type objectType)
        {
            return objectType == typeof(DateTime) || objectType == typeof(DateTimeOffset);
        }

        public override object ReadJson(
            JsonReader reader,
            System.Type objectType,
            object existingValue,
            JsonSerializer serializer)
        {
            JToken jtoken = JToken.Load(reader);
            if (jtoken.Type == JTokenType.String || jtoken.Type == JTokenType.Date)
            {
                string str = jtoken.ToObject<string>();
                System.Type type = typeof(T);
                if (type == typeof(DateTime) || type == typeof(DateTime?))
                    return (object)DateTime.Parse(str);
                if (type == typeof(DateTimeOffset))
                    return (object)DateTimeOffset.Parse(str);
            }
            return (object)null;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            if (value == null)
            {
                writer.WriteNull();
            }
            else
            {
                DateTime dateTime = (DateTime)value;
                serializer.Serialize(writer, dateTime.Kind == DateTimeKind.Utc ? (object)dateTime.ToString(this.Format) : (object)dateTime.ToUniversalTime().ToString(this.Format));
            }
        }
    }
}
