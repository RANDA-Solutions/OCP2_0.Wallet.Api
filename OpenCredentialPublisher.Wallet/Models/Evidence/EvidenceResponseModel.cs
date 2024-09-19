using System;

namespace OpenCredentialPublisher.Wallet.Models.Evidence
{
    public record EvidenceResponseModel
    {
        protected EvidenceResponseModel(Data.Custom.EFModels.Evidence evidence)
        {
            EvidenceUrl = evidence.EvidenceUrl;
            Name = string.IsNullOrWhiteSpace(evidence.Name) ? "Unspecified" : evidence.Name;
            CreatedAt = evidence.CreatedAt.DateTime;
        }

        public string EvidenceUrl { get; set; }
        public string Name { get; }
        public DateTime CreatedAt { get; }

        public static EvidenceResponseModel FromModel(Data.Custom.EFModels.Evidence evidence)
        {
            return new EvidenceResponseModel(evidence);
        }
    }
}