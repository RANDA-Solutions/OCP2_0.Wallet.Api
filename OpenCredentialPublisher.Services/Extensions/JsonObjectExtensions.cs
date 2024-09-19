using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text;
using System.Text.Json.Nodes;

namespace OpenCredentialPublisher.Services.Extensions
{
    public static class JsonObjectExtensions
    {
        public static string GetThreadId(this JsonObject jsonObject)
        {
            return jsonObject["~thread"]["thid"].ToString();
        }

        public static string GetRelationshipDID(this JsonObject jsonObject)
        {
            return jsonObject["myDID"].ToString();
        }
        public static string GetDID(this JsonObject jsonObject)
        {
            return jsonObject["did"].ToString();
        }

        public static string GetVerKey(this JsonObject jsonObject)
        {
            return jsonObject["verKey"].ToString();
        }

        public static string GetInviteUrl(this JsonObject jsonObject)
        {
            return jsonObject["inviteURL"].ToString();
        }

        public static string GetSchemaId(this JsonObject jsonObject)
        {
            return jsonObject["schemaId"].ToString();
        }

        public static string GetCredentialDefinitionId(this JsonObject jsonObject)
        {
            return jsonObject["credDefId"].ToString();
        }

        /// <summary>
        /// Get and return JSON value by key without \\"
        /// </summary>
        /// <param name="json">This object</param>
        /// <param name="key">Key</param>
        /// <returns></returns>
        public static string GetString(this JsonObject json, string key)
        {
            try
            {
                return json[key].ToString().Trim('"');
            }
            catch
            {
                return null;
            }
        }

        public static string AsString(this JsonObject json)
        {
            try
            {
                return json.ToString().Trim('"');
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Get and return JSON value by key without \\"
        /// </summary>
        /// <param name="json">This object</param>
        /// <param name="key">Key</param>
        /// <returns></returns>
        public static string GetString(this JsonValue json, string key)
        {
            try
            {
                return json.GetString(key).Trim('"');
            }
            catch
            {
                return null;
            }
        }
    }
}
