using System;
using System.Collections.Immutable;
using System.Linq;
using OpenCredentialPublisher.Data.Custom.EFModels;

namespace OpenCredentialPublisher.Wallet.Models.Shares
{
    public record ShareDetailsResponseModel
    {
        protected ShareDetailsResponseModel(string userId,Share share)
        {
            ShareId = share.ShareId;
            Email = share.Email;
            Description = share.Description;
            CreatedAt = share.CreatedAt.DateTime;
            CanDelete = share.UserId == userId;

            VerifiableCredentialIds = share.ShareVerifiableCredentials
                .Select(svc => svc.VerifiableCredentialId)
                .ToImmutableList();
            CredentialCollectionIds = share.ShareCredentialCollections
                .Select(scc => scc.CredentialCollectionId)
                .ToImmutableList();
        }

        public long ShareId { get; }
        public string Email { get; }
        public bool CanDelete { get; }
        public string Description { get; }
        public DateTime CreatedAt { get; }

        public IImmutableList<long> VerifiableCredentialIds { get; } = ImmutableList<long>.Empty;
        public IImmutableList<long> CredentialCollectionIds { get; } = ImmutableList<long>.Empty;

        public static ShareDetailsResponseModel FromModel(string userId, Share share)
        {
            return new ShareDetailsResponseModel(userId, share);
        }
    }
}
