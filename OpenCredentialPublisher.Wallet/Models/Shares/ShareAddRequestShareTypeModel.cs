using System.Collections.Generic;
using OpenCredentialPublisher.Data.Custom.Commands;

namespace OpenCredentialPublisher.Wallet.Models.Shares
{
    public class ShareAddRequestShareTypeModel
    {
        public string ShareType { get; init; }

        public List<long> VerifiableCredentialIds { get; init; }
        public List<long> CredentialCollectionIds { get; init; }

        public ShareAddShareTypeCommand ToCommand()
        {
            return new ShareAddShareTypeCommand(VerifiableCredentialIds, CredentialCollectionIds, ShareType);
        }
    }
}