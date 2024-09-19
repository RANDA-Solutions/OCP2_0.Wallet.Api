namespace OpenCredentialPublisher.Wallet.Models.Account
{
    public enum AccountSetupStatusEnum
    {
        VerifyEmailNeeded = 10,
        AccountSetupNeeded = 20,
        AccountComplete = 30,
        AccountNotFound = 40,
        AccessCodeExpired = 50
    }
}