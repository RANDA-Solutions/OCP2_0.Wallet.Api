using System;
using System.Collections.Generic;
using OpenCredentialPublisher.Data.Models;
using OpenCredentialPublisher.Shared.Interfaces;

namespace OpenCredentialPublisher.Data.Custom.EFModels
{
    /// <summary>
    /// OAuth 2.0 data for an application user and resource server.
    /// </summary>
    public class Authorization : IBaseEntity
    {
        /// <summary>
        /// Access token.
        /// </summary>
        public string AccessToken { get; set; }

        /// <summary>
        /// Authorization code. Only has a value during ACG flow.
        /// </summary>
        public string AuthorizationCode { get; set; }

        /// <summary>
        /// PKCE code verifier. Only has a value during ACG flow.
        /// </summary>
        public string CodeVerifier { get; set; }

        public string Endpoint { get; set; }

        public string Id { get; set; }

        public string Method { get; set; }

        public string Payload { get; set; }

        /// <summary>
        /// Refresh token (if issued by authorization server).
        /// </summary>
        public string RefreshToken { get; set; }

        /// <summary>
        /// Scopes this user has permission to use.
        /// </summary>
        public List<string> Scopes { get; set; }

        /// <summary>
        /// Resource server these credentials work with.
        /// </summary>
        public Source Source { get; set; }

        /// <summary>
        /// Foreign key back to the resource server.
        /// </summary>
        public long SourceId { get; set; }

        public string UserId { get; set; }
        public ApplicationUser User { get; set; }

        public DateTime ValidTo { get; set; }
        public bool IsDeleted { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset? ModifiedAt { get; set; }

        public void Delete()
        {
            IsDeleted = true;
            ModifiedAt = DateTime.UtcNow;
        }
    }
}
