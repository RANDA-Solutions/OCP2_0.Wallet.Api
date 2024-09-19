namespace OpenCredentialPublisher.Data.Custom.Results
{
    public record PublicShareValidateResult(long ShareId, string Hash, string Code, string ShareFromUserId);
}