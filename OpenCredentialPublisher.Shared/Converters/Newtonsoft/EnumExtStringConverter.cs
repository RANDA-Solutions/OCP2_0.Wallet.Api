using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace OpenCredentialPublisher.Shared.Converters.Newtonsoft
{
    public class EnumExtStringConverter<T> : JsonConverter where T : Enum
    {
        private const string Pattern = "(ext:)[a-z|A-Z|0-9|.|-|_]+";

        public override bool CanConvert(System.Type objectType) => objectType == typeof(string);

        public override object ReadJson(
            JsonReader reader,
            System.Type objectType,
            object existingValue,
            JsonSerializer serializer)
        {
            JToken jtoken = JToken.Load(reader);
            System.Type enumType = typeof(T);
            if (enumType.IsEnum && jtoken.Type == JTokenType.String)
            {
                string[] names = Enum.GetNames(enumType);
                string input = jtoken.Value<string>();
                if (((IEnumerable<string>)names).Contains<string>(input) || Regex.IsMatch(input, "(ext:)[a-z|A-Z|0-9|.|-|_]+"))
                    return (object)input;
            }
            return (object)"InvalidValue";
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            string[] names = Enum.GetNames(typeof(T));
            string input = (string)value;
            if (((IEnumerable<string>)names).Contains<string>(input) || Regex.IsMatch(input, "(ext:)[a-z|A-Z|0-9|.|-|_]+"))
                serializer.Serialize(writer, value);
            else
                serializer.Serialize(writer, (object)("ext:" + input));
        }
    }

}
