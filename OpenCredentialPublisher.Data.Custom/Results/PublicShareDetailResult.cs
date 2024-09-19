using System.Collections.Immutable;
using OpenCredentialPublisher.Data.Custom.EFModels;

namespace OpenCredentialPublisher.Data.Custom.Results
{
    public class PublicShareDetailResult
    {
        public Share Share { get; set; }
        public IImmutableList<long> VerifiableCredentialIds { get; set; }
    }
}