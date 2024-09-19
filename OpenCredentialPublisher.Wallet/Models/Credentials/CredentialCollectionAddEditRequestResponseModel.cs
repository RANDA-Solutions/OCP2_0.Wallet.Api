using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using OpenCredentialPublisher.Data.Custom.Commands;
using OpenCredentialPublisher.Data.Custom.EFModels;
using OpenCredentialPublisher.Wallet.Models.Shared;

namespace OpenCredentialPublisher.Wallet.Models.Credentials
{
    public class CredentialCollectionAddEditRequestResponseModel
    {
        public CredentialCollectionAddEditRequestResponseModel()
        {
            VerifiableCredentialItems = new List<ListItemRequestResponseModel>();
        }

        private CredentialCollectionAddEditRequestResponseModel(long credentialCollectionId, string name, string description, List<ListItemRequestResponseModel> verifiableCredentialItems, int shareCount, DateTime createdAt, bool canDelete)
        {
            CredentialCollectionId = credentialCollectionId;
            Name = name;
            Description = description;
            VerifiableCredentialItems = verifiableCredentialItems;
            ShareCount = shareCount;
            CreatedAt = createdAt;
            CanDelete = canDelete;
        }

        public long CredentialCollectionId { get; init; }
        public int ShareCount { get; init; }

        [MaxLength(100)]
        public string Name { get; init; }
        [MaxLength(1000)]
        public string Description { get; init; }

        public DateTime CreatedAt { get; init; }
        public bool CanDelete { get; init; }

        public List<ListItemRequestResponseModel> VerifiableCredentialItems { get; init; }

        public static CredentialCollectionAddEditRequestResponseModel FromModel(string userId, CredentialCollection cc)
        {
            var verifiableCredentialItems = cc.CredentialCollectionVerifiableCredentials.Select(cvc =>
                new ListItemRequestResponseModel(cvc.VerifiableCredentialId, cvc.VerifiableCredential.Name)).ToList();
            var shareCount = cc.ShareCredentialCollections.Count;
            var canDelete = userId == cc.UserId;

            return new CredentialCollectionAddEditRequestResponseModel(cc.CredentialCollectionId, cc.Name, cc.Description,
                verifiableCredentialItems, shareCount, cc.CreatedAt.DateTime, canDelete);
        }

        public CredentialCollectionAddCommand ToAddCommand()
        {
            return new CredentialCollectionAddCommand(Name, Description,
                VerifiableCredentialItems.Select(vci => vci.Id).ToList());
        }

        public CredentialCollectionSaveCommand ToSaveCommand()
        {
            return new CredentialCollectionSaveCommand(CredentialCollectionId, Name, Description,
                VerifiableCredentialItems.Select(vci => vci.Id).ToList());
        }
    }
}