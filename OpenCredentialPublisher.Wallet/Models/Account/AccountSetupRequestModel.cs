namespace OpenCredentialPublisher.Wallet.Models.Account
{
    public class AccountSetupRequestModel
    {
        public string Email { get; set; }
        public string AccessCode { get; set; }
        public string DisplayName { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
    }
}
