using Newtonsoft.Json.Linq;

namespace OpenCredentialPublisher.Data.Custom.Settings
{
    public class MailSettings
    {
        public bool UseSSL { get; set; }
        public string Server { get; set; }
        public int Port { get; set; }
        public string User { get; set; }
        public string Password { get; set; }
        public string CredentialsFrom { get; set; }
        public string From { get; set; }
        public bool RedirectToInternal { get; set; }
        public string RedirectAddress { get; set; }
        public string SupportEmail { get; set; }
    }
}
