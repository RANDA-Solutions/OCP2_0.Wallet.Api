using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Linq;
using NJsonSchema;
using NJsonSchema.NewtonsoftJson.Generation;
using OpenCredentialPublisher.Data.Abstracts;
using OpenCredentialPublisher.Data.Custom.CredentialModels;
using OpenCredentialPublisher.Services.Extensions;
using OpenCredentialPublisher.Services.Interfaces;

// ReSharper disable InconsistentNaming

namespace OpenCredentialPublisher.Services.Implementations
{
    public class SchemaService : ISchemaService
    {
        private static readonly NewtonsoftJsonSchemaGeneratorSettings _jsonSchemaSettings = new()
        {
            //AlwaysAllowAdditionalObjectProperties = true, // this allows anything to validate
            FlattenInheritanceHierarchy = true, // makes all inherited properties appear directly on object for schema
        };
        private static readonly Type[] _clr2Types;


        static SchemaService()
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

        public Type GetTypeForJson(string[] jsonTypes)
        {
            var assemblyType = _clr2Types.FirstOrDefault(ct =>
                jsonTypes.Any(jt => ct.Name.Equals($"{jt}Model", StringComparison.OrdinalIgnoreCase)));

            // error if matchingType not found
            if (assemblyType == null) throw new ArgumentException($"Unable to find type for {string.Join(",", jsonTypes)}");

            return assemblyType;
        }

        public SchemaResult Validate(string jsonData)
        {
            var jObject = JObject.Parse(jsonData);
            var schemaResult = new SchemaResult();

            var errors = new List<string>();
            ValidateObject(jObject, errors, "");

            foreach (var error in errors)
            {
                schemaResult.ErrorMessages.Add(error);
            }

            return schemaResult;
        }

        private void ValidateObject(JObject jObject, List<string> errors, string currentPath)
        {
            // every object in CLR2 has a property type which is an array
            if (jObject["type"] is JArray typesJArray)
            {
                var jsonTypes = typesJArray.Select(j => j.ToString()).ToArray();

                var assemblyType = GetTypeForJson(jsonTypes);

                // load JSON schema for this type
                var jsonSchema = JsonSchema.FromType(assemblyType, _jsonSchemaSettings);

                // validate each property of JSON individually as we need
                // to check for JObjects and find their corresponding .NET type
                foreach (var jProperty in jObject.Properties())
                {
                    var propertyPath = string.IsNullOrEmpty(currentPath) ? jProperty.Name : $"{currentPath}.{jProperty.Name}";

                    // for objects, we recursively call this function
                    if (jProperty.Value.Type == JTokenType.Object)
                    {
                        ValidateObject((JObject)jProperty.Value, errors, propertyPath);
                    }
                    // special case to handle arrays of objects
                    else if (jProperty.Value.Type == JTokenType.Array)
                    {
                        // iterate over all array values and recursively call this function
                        foreach (var jPropertyValue in (JArray)jProperty.Value)
                        {
                            if (jPropertyValue.Type == JTokenType.Object)
                                ValidateObject((JObject)jPropertyValue, errors, propertyPath);
                            else
                                ValidateProperty(jsonSchema, jProperty, errors, propertyPath);
                        }
                    }
                    else
                    {
                        // else validate normal type properties
                        ValidateProperty(jsonSchema, jProperty, errors, propertyPath);
                    }
                }
            }
        }

        private void ValidateProperty(JsonSchema jsonSchema, JProperty jProperty, List<string> errors, string propertyPath)
        {
            // skip properties not in our schema as CLR2 allows extension
            if (jsonSchema.Properties.TryGetValue(jProperty.Name, out var propertySchema))
            {
                var validationErrors = propertySchema.Validate(jProperty.Value);

                errors.AddRange(validationErrors.Select(validationError => $"{propertyPath}\n{validationError}"));
            }
        }

        public class SchemaResult : GenericModel
        {
            public bool IsValid => !HasError;
        }
    }
}
