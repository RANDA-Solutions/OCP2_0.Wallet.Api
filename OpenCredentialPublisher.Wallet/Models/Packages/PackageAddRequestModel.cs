using Microsoft.AspNetCore.Http;

namespace OpenCredentialPublisher.Wallet.Models.Packages
{
    public class PackageAddRequestModel
    {
        public IFormFile PackageFile { get; set; }
    }
}
