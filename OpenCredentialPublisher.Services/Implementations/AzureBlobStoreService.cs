using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.Extensions.Options;
using OpenCredentialPublisher.Data.Options;
using OpenCredentialPublisher.Services.Constants;
using OpenCredentialPublisher.Services.Interfaces;
using System;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace OpenCredentialPublisher.Services.Implementations
{
    public class AzureBlobStoreService : IFileStoreService
    {
        public const string ProfilePictureContainerName = AzureConstants.ProfilePictureContainerName;

        private readonly AzureBlobOptions _options;
        public AzureBlobStoreService(IOptions<AzureBlobOptions> options)
        {
            _options = options?.Value ?? throw new ArgumentNullException(nameof(options));
        }

        public async Task<string> StoreAsync(string filename, string contents, string blobContainerName)
        {
            // Get a reference to a container
            var container = new BlobContainerClient(_options.StorageConnectionString, blobContainerName);
            await container.CreateIfNotExistsAsync();

            var blob = container.GetBlobClient(filename);

            using var ms = new MemoryStream();
            var writer = new StreamWriter(ms);
            await writer.WriteAsync(contents);
            await writer.FlushAsync();
            ms.Position = 0;

            await blob.UploadAsync(ms);

            return filename;
        }

        public async Task<string> SaveToBlobAsync(string containerName, string fileId, string extension, byte[] contents, PublicAccessType publicAccessType = PublicAccessType.None)
        {
            var container = new BlobContainerClient(_options.StorageConnectionString, containerName);
            if (!await container.ExistsAsync())
            {
                await container.CreateIfNotExistsAsync();
                await container.SetAccessPolicyAsync(publicAccessType);
            }
            var date = DateTimeOffset.UtcNow;
            var filename = $"{date:yyyy/MM/dd}/{fileId}.{extension}";
            var location = $"https://{container.AccountName}.blob.core.windows.net/{containerName}/{filename}";

            var blob = container.GetBlobClient(filename);
            using var ms = new MemoryStream(contents);
            await blob.UploadAsync(ms);
            return location;
        }

        public async Task<string> StoreAsync(string filename, byte[] contents, string blobContainerName)
        {
            // Get a reference to a container
            var container = new BlobContainerClient(_options.StorageConnectionString, blobContainerName);
            await container.CreateIfNotExistsAsync();

            var blob = container.GetBlobClient(filename);

            using var ms = new MemoryStream(contents, false);
            await blob.UploadAsync(ms);

            return filename;
        }

        public async Task<string> DownloadAsStringAsync(string filename, string blobContainerName)
        {
            var bytes = await DownloadAsync(filename, blobContainerName);

            string utfString = Encoding.UTF8.GetString(bytes, 0, bytes.Length);

            return utfString;
        }

        public async Task<byte[]> DownloadAsync(string filename, string blobContainerName)
        {
            // Get a reference to a container named "sample-container" and then create it
            var container = new BlobContainerClient(_options.StorageConnectionString, blobContainerName.ToLower());
            await container.CreateIfNotExistsAsync();

            var blob = container.GetBlobClient(filename);

            BlobDownloadInfo download = await blob.DownloadAsync();

            using var ms = new MemoryStream();
            await download.Content.CopyToAsync(ms);

            return ms.ToArray();
        }

        public async Task<byte[]> DownloadAsync(string filename, string blobContainerName, bool isCustom)
        {
            var connectionString = _options.StorageConnectionString;

            var container = new BlobContainerClient(connectionString, blobContainerName.ToLower());

            var blob = container.GetBlobClient(filename);

            BlobDownloadInfo download = await blob.DownloadAsync();

            using var ms = new MemoryStream();
            await download.Content.CopyToAsync(ms);

            return ms.ToArray();
        }

        public static (string account, string container, string filename, bool isCustom) ParseStorageAccountUrl(string url)
        {
            if (string.IsNullOrEmpty(url))
                throw new ArgumentNullException("url");

            var isCustom = !url.Contains("blob.core.windows.net");

            const string commonExp = @"https://(?<account>\w+)[\w-\.]*/(?<container>[\w-]+)/(?<filename>.*)";
            const string customExp = @"https://(?<account>[\w-\.]+)/(?<container>[\w-]+)/(?<filename>.*)";
            var regex = new Regex(isCustom ? customExp : commonExp);
            var match = regex.Match(url);
            if (match.Success)
            {
                var account = match.Groups["account"];
                var container = match.Groups["container"];
                var filename = match.Groups["filename"];
                return (account.Value, container.Value, filename.Value, isCustom);
            }
            throw new ArgumentException("Only valid storage account urls are allowed");
        }

    }
}
