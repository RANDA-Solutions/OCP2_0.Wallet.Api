using System;
using System.IO;
using System.Threading.Tasks;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using OpenCredentialPublisher.Data.Options;

namespace OpenCredentialPublisher.Services.Implementations
{
    public class ProfileImageService
    {
        private const string BlobContainerName = "ocp-profile-images";
        private readonly ILogger<ProfileImageService> _logger;
        private readonly PublicBlobOptions _publicBlobOptions;
        public ProfileImageService(IOptions<PublicBlobOptions> publicBlobOptions, ILogger<ProfileImageService> logger)
        {
            _logger = logger;
            _publicBlobOptions = publicBlobOptions?.Value;
        }

        public async Task<string> SaveImageToBlobAsync(string userId, string existingImageUrl, byte[] newImageBytes, string extension = ".png")
        {
            var container = new BlobContainerClient(_publicBlobOptions.StorageConnectionString, BlobContainerName);
            if (!await container.ExistsAsync())
            {
                await container.CreateIfNotExistsAsync();
                await container.SetAccessPolicyAsync(PublicAccessType.Blob);
            }

            // remove existing
            if (!string.IsNullOrWhiteSpace(existingImageUrl))
            {
                await DeleteImageFromBlobAsync(existingImageUrl);
            }

            // handle extension format
            if (!string.IsNullOrWhiteSpace(extension) && !extension.StartsWith('.'))
                extension = extension.Insert(0, ".");

            var date = DateTime.UtcNow;
            var imageId = Guid.NewGuid();
            var filename = $"{date:yyyy/MM/dd}/{imageId}{extension}";
            var location = string.IsNullOrWhiteSpace(_publicBlobOptions.CustomDomainName) ? $"https://{container.AccountName}.blob.core.windows.net/{BlobContainerName}/{filename}" : $"https://{_publicBlobOptions.CustomDomainName}/{BlobContainerName}/{filename}";

            var blob = container.GetBlobClient(filename);
            using var ms = new MemoryStream(newImageBytes);
            await blob.UploadAsync(ms);

            // try to find correct content type for header (e.g., image/png)
            var ctProvider = new FileExtensionContentTypeProvider();
            if (!ctProvider.TryGetContentType(filename, out var contentType))
            {
                contentType = "application/octet-stream";
            }

            // since never update, set cache control so browser doesn't have to download repeatedly
            var headers = new BlobHttpHeaders
            {
                ContentType = contentType,
                CacheControl = "max-age=2592000"  // cache for 30 days
            };
            await blob.SetHttpHeadersAsync(headers);

            return location;
        }

        public async Task<bool> DeleteImageFromBlobAsync(string location)
        {
            var container = new BlobContainerClient(_publicBlobOptions.StorageConnectionString, BlobContainerName);
            if (!await container.ExistsAsync())
            {
                await container.CreateIfNotExistsAsync();
                await container.SetAccessPolicyAsync(PublicAccessType.Blob);
            }
            try
            {
                var storageAccount = AzureBlobStoreService.ParseStorageAccountUrl(location);
                var blob = container.GetBlobClient(storageAccount.filename);
                return await blob.DeleteIfExistsAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"There was a problem deleting {location} from {BlobContainerName}");
            }
            return false;
        }
    }
}
