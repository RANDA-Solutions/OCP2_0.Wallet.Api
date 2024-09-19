namespace OpenCredentialPublisher.Data.Options
{
    public class AzureAppConfigConfiguration
    {
        public const string SectionName = "AzureAppConfig";
        public string Label { get; set; }
        public string ConnectionString { get; set; }
    }
}
