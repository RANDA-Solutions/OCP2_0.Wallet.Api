using System;
using OpenCredentialPublisher.Data.Custom.EFModels;

namespace OpenCredentialPublisher.Wallet.Models.Shares
{
    public class ShareListResponseModel
    {
        protected ShareListResponseModel(Share share)
        {
            ShareId = share.ShareId;
            Email = share.Email;
            Description = share.Description;
            ShareType = share.ShareType;
            CreatedAt = share.CreatedAt.DateTime;
            CredentialCount = share.TotalCredentialCount;
        }

        public long ShareId { get; }
        public string Email { get; }
        public string Description { get; }
        public string ShareType { get; }
        public int CredentialCount { get; }
        public DateTime CreatedAt { get; }

        public static ShareListResponseModel FromModel(Share share)
        {
            return new ShareListResponseModel(share);
        }
    }
}
