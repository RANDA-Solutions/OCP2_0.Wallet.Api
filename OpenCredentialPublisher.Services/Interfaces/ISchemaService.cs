using OpenCredentialPublisher.Services.Implementations;
using System;

namespace OpenCredentialPublisher.Services.Interfaces
{
    public interface ISchemaService
    {
        Type GetTypeForJson(string[] jsonTypes);
        SchemaService.SchemaResult Validate(string jsonData);
    }
}