using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using OpenCredentialPublisher.Data.Custom.Commands;

namespace OpenCredentialPublisher.Wallet.Models.Shares
{
    public class ShareAddRequestModel
    {
        public ShareAddRequestModel()
        {
            
        }

        [EmailAddress]
        [MaxLength(256)]
        public string Email { get; init; }

        [MaxLength(1000)]
        public string Description { get; init; }
        
        public List<long> VerifiableCredentialIds { get; init; }
        public List<long> CredentialCollectionIds { get; init; }

        public ShareAddCommand ToCommand()
        {
            return new ShareAddCommand(Email, Description, VerifiableCredentialIds, CredentialCollectionIds);
        }
    }
}