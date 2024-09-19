namespace OpenCredentialPublisher.Wallet.Models.Account
{
    public class VerifyEmailResponseModel
    {
        public string Email { get; set; }
        public AccountSetupStatusEnum Status { get; set; }
    }
}
