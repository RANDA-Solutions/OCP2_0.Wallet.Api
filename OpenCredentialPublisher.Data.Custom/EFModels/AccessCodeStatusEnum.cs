namespace OpenCredentialPublisher.Data.Custom.EFModels
{
    public enum AccessCodeStatusEnum
    {
        Valid = 10,
        Invalid = 20,
        Expired = 30,
        NotFound = 40,
        AccountComplete = 50,
        AccountUpdateFailed = 60
    }
}
