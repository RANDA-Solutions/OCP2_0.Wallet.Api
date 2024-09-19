namespace OpenCredentialPublisher.Data.Custom.Options
{
    public class SiteSettingsOptions
    {
        public const string Section = "SiteSettings";
        public bool ShowAddCredential { get; set; }
        public bool ShowFooter { get; set; }
        public string ContactUsUrl { get; set; }
        public string PrivacyPolicyUrl { get; set; }
        public string TermsOfServiceUrl { get; set; }
        public string FaqsUrl { get; set; }

        public int SessionTimeout { get; set; }
        public string AllowedOrigins { get; set; }
        public string SpaClientUrl { get; set; }
        public bool EnableEditEmail { get; set; }
        public int RevocationCacheDurationInSeconds { get; set; }

        public string GetRevocationCacheDurationHeaderString()
        {
            return $"private, max-age={RevocationCacheDurationInSeconds}";
        }

    }
}
