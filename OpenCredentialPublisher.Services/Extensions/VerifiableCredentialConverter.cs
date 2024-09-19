using System;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using OpenCredentialPublisher.Data.Custom.CredentialModels;

namespace OpenCredentialPublisher.Services.Extensions
{
    public class VerifiableCredentialModelConverter : JsonConverter<VerifiableCredentialModel>
    {
        // ReSharper disable once InconsistentNaming
        private static readonly Type[] _clr2Types;

        static VerifiableCredentialModelConverter()
        {
            // Get the assembly where VerifiableCredential is defined
            var markerType = typeof(VerifiableCredentialModel);
            var assembly = markerType.Assembly;

            // Get all types in the specified assembly that inherit from VerifiableCredential
            _clr2Types = assembly!.GetTypes()
                .Where(t => !t.IsAnonymousType())
                .Where(t => !string.IsNullOrEmpty(t.Namespace) && t.Namespace.Equals(markerType.Namespace) && !t.IsAbstract)
                .ToArray();
        }

        
        public override VerifiableCredentialModel Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            using var jsonDocument = JsonDocument.ParseValue(ref reader);
            var jsonDocumentRootElement = jsonDocument.RootElement;
            var originalJson = jsonDocumentRootElement.GetRawText();
            
            if (jsonDocumentRootElement.TryGetProperty("type", out var jsonElement) && jsonElement.ValueKind == JsonValueKind.Array)
            {
                var typeArray = jsonElement.EnumerateArray().Select(t => t.GetString()).ToList();

                var assemblyType = _clr2Types.FirstOrDefault(ct =>
                    typeArray.Any(jt => ct.Name.Equals($"{jt}Model", StringComparison.OrdinalIgnoreCase)));

                // error if matchingType not found
                if (assemblyType == null) throw new ArgumentException($"Unable to find type for {string.Join(",", typeArray)}");

                var vc = (VerifiableCredentialModel)JsonSerializer.Deserialize(jsonDocumentRootElement.GetRawText(), assemblyType, options);

                vc!.OriginalJson = originalJson;
                return vc;
            }

            // Create new JsonSerializerOptions without the custom converter for fallback
            var fallbackOptions = new JsonSerializerOptions(options);
            fallbackOptions.Converters.Remove(this);

            // If no specific type is matched, fallback to VerifiableCredential
            var vcFallBack = JsonSerializer.Deserialize<VerifiableCredentialModel>(jsonDocumentRootElement.GetRawText(), fallbackOptions);
            vcFallBack.OriginalJson =  originalJson;
            return vcFallBack;
        }

        public override void Write(Utf8JsonWriter writer, VerifiableCredentialModel value, JsonSerializerOptions options)
        {
            JsonSerializer.Serialize(writer, value, value.GetType(), options);
        }
    }
}
