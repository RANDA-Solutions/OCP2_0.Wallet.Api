using System;
using System.Collections.Generic;
using OpenCredentialPublisher.Data.Models.Enums;
using OpenCredentialPublisher.Shared.Interfaces;

namespace OpenCredentialPublisher.Data.Custom.EFModels
{
    /// <summary>
    /// Represents a Resource Server that has implemented the CLR spec, and its
    /// Authorization Server.
    /// </summary>
    public class Source : IBaseEntity
    {
        /// <summary>
        /// All the authorizations tied to this resource server.
        /// </summary>
        public List<Authorization> Authorizations { get; set; }

        /// <summary>
        /// OAuth 2.0 client identifier string.
        /// </summary>
        public string ClientId { get; set; }

        /// <summary>
        /// OAuth 2.0 client secret string.
        /// </summary>
        public string ClientSecret { get; set; }


        /// <summary>
        /// Primary key.
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// The name of the resource server (also in the Discovery Document).
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The scopes the authorization server and resource server support.
        /// </summary>
        public string Scope { get; set; }

        /// <summary>
        /// The base URL for the resource server.
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// BitMap of entitytypes the source provides
        /// </summary>
        public SourceTypeEnum SourceTypeId { get; set; }
        public bool IsDeletable { get; set; }
        public bool IsDeleted { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset? ModifiedAt { get; set; }

        public void Delete()
        {
            IsDeleted = true;
            ModifiedAt = DateTime.UtcNow;

            foreach (var authorization in Authorizations)
            {
                authorization.Delete();
            }
        }
    }
}
