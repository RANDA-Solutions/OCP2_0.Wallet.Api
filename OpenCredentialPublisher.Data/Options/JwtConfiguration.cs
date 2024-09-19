namespace OpenCredentialPublisher.Data.Options
{
    public class JwtConfiguration
    {
        public const string SectionName = "JWT";

        public string ValidAudience { get; set; }
        public string ValidIssuer { get; set; }
        public string Secret { get; set; }
    }
}
