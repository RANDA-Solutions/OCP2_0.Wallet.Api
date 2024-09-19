namespace OpenCredentialPublisher.Wallet.Models.Account
{
    public class AccountProfileResponseModel
    {

        public string DisplayName { get; set; }
        public string PhoneNumber { get; set; }

        public string Email { get; set; }
        public bool IsEmailConfirmed { get; set; }
        public string ProfileImageUrl { get; set; }
        public bool HasProfileImage { get; set; }

        public bool EnableEditEmail { get; set; }
    }
}
