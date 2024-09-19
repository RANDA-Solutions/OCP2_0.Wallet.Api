namespace OpenCredentialPublisher.Wallet.Models.Shared
{
    public sealed class VerifyEmailRequestModel
    {
        public string UserId { get; set; }
        public string Email { get; set; }
        public string Code { get; set; }
    }
}
