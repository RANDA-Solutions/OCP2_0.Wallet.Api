using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;

namespace OpenCredentialPublisher.Shared.Converters.Json
{
    public class EnumExtStringConverter<T> : JsonConverter<string> where T : Enum
    {
        private const string Pattern = "^ext:(.+)$";

        public override string Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType != JsonTokenType.String)
            {
                throw new JsonException();
            }

            var enumString = reader.GetString();

            if (string.IsNullOrEmpty(enumString))
                return null;

            Type enumType = typeof(T);

            // does our string directly match an enum value?
            if (Enum.IsDefined(enumType, enumString))
            {
                return enumString;
            }

            // handle "ext:" prefix
            // NOTE: these won't be in the enum
            var match = Regex.Match(enumString, Pattern);
            return match.Success ? match.Groups[1].Value.Trim() : null;
        }

        public override void Write(Utf8JsonWriter writer, string value, JsonSerializerOptions options)
        {
            Type enumType = typeof(T);

            if (Enum.IsDefined(enumType, value) || Regex.IsMatch(value, Pattern))
            {
                writer.WriteStringValue(value);
            }
            else
            {
                writer.WriteStringValue("ext:" + value);
            }
        }
    }
}