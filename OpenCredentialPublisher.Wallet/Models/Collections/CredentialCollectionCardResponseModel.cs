using System;
using OpenCredentialPublisher.Data.Custom.EFModels;

namespace OpenCredentialPublisher.Wallet.Models.Collections
{
    public record CredentialCollectionCardResponseModel(
        long CredentialCollectionId,
        string Name,
        string Description,
        int ShareCount,
        DateTime CreatedAt,
        bool CanDelete)
    {
        public static CredentialCollectionCardResponseModel FromModel(string userId, CredentialCollection cc)
        {
            var shareCount = cc.ShareCredentialCollections.Count;

            return new CredentialCollectionCardResponseModel(cc.CredentialCollectionId, cc.Name, cc.Description,
                shareCount, cc.CreatedAt.DateTime, cc.UserId == userId);
        }
    }

}