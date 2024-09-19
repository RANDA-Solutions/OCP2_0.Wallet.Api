namespace OpenCredentialPublisher.Data.Custom.Results
{
    public class RevocationResult
    {
        public bool IsRevoked { get; set; }
        public string RevokedReason { get; set; }
    }
}
