namespace OpenCredentialPublisher.Wallet.Models.Account
{
    public class RegisterConfirmResponseModel
    {
        public string Email { get; set; }
        public bool DisplayConfirmAccountLink { get; set; }
        public string EmailConfirmationUrl { get; set; }
    }

}
