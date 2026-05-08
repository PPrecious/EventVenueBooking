using Azure.Storage.Blobs;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace EventVenueBooking.Services
{
    public class BlobService
    {
        private readonly string _connectionString;

        public BlobService(IConfiguration config)
        {
            _connectionString = config.GetConnectionString("AzureBlobStorage")
                                ?? throw new Exception("Azure Blob Storage connection string is missing.");
        }

        public async Task<string> UploadFileAsync(IFormFile file)
        {
            var container = new BlobContainerClient(_connectionString, "venue-images");

            await container.CreateIfNotExistsAsync();

            var fileName = Guid.NewGuid().ToString() + "_" + file.FileName;

            var blob = container.GetBlobClient(fileName);

            using (var stream = file.OpenReadStream())
            {
                await blob.UploadAsync(stream, overwrite: true);
            }

            return blob.Uri.ToString();
        }
    }
}