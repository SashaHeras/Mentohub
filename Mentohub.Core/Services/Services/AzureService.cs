using Microsoft.AspNetCore.Http;
using Microsoft.WindowsAzure.Storage;
using Mentohub.Core.Repositories.Repositories;
using Mentohub.Domain.Entities;
using Mentohub.Core.Services.Interfaces;
using Microsoft.AspNetCore.Hosting.Server;
using Azure.Storage.Blobs;
using Microsoft.WindowsAzure.Storage.Blob;
using Azure;
using System.IO;

namespace Mentohub.Core.Services.Services
{
    public class AzureService : IAzureService
    {
        private readonly string _connectionString = "DefaultEndpointsProtocol=https;AccountName=mystudystorage;AccountKey=F2DhOdWx3qBaoImpVaDkLDVCyErlLVghvKL5kcxYLL9V7KsOQobaH8wWSh4m48ACDDK/lnsyzd3Q+AStciFc5Q==;EndpointSuffix=core.windows.net";

        public AzureService() { 

        }

        /// <summary>
        /// Uploading of file on Azure
        /// </summary>
        /// <param name="file"></param>
        /// <returns>Name of file in Azure with was generated as Guid</returns>
        public async Task<string> SaveInAsync(IFormFile file)
        {
            string newName = Guid.NewGuid().ToString().Replace("-", "");

            try
            {
                var storageAccount = CloudStorageAccount.Parse(_connectionString);
                var blobClient = storageAccount.CreateCloudBlobClient();
                var container = blobClient.GetContainerReference("test");

                await container.CreateIfNotExistsAsync();

                var blockBlob = container.GetBlockBlobReference(newName);

                await using (var stream = file.OpenReadStream())
                {
                    await blockBlob.UploadFromStreamAsync(stream);
                }

                return newName;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Deleting file from Azure by name
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public async Task<bool> DeleteFromAzure(string name)
        {
            try
            {
                var storageAccount = CloudStorageAccount.Parse(_connectionString);
                var blobClient = storageAccount.CreateCloudBlobClient();
                var container = blobClient.GetContainerReference("test");
                var blockBlob = container.GetBlockBlobReference(name);

                if (await blockBlob.DeleteIfExistsAsync())
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<IFormFile> CopyVideoFromBlob(string name)
        {
            var blobServiceClient = new BlobServiceClient(_connectionString);
            var blobContainerClient = blobServiceClient.GetBlobContainerClient("test");
            var blobClient = blobContainerClient.GetBlobClient(name);

            try
            {
                var response = await blobClient.DownloadAsync();
                string fileName = string.Empty;

                using (var fileStream = File.OpenWrite(name + ".mp4"))
                {
                    await response.Value.Content.CopyToAsync(fileStream);
                    fileName = fileStream.Name;

                    fileStream.Close();
                }

                var obj = CreateFormFile(fileName);

                return obj;
            }
            catch (RequestFailedException ex)
            {
                Console.WriteLine($"Error getting blob: {ex.Message}");
            }

            return null;
        }

        public IFormFile CreateFormFile(string filePath)
        {
            // Open the video file using a FileStream
            using (FileStream fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
            {
                // Create a MemoryStream to store the file contents
                using (MemoryStream memoryStream = new MemoryStream())
                {
                    // Copy the file contents to the MemoryStream
                    fileStream.CopyTo(memoryStream);

                    // Create an IFormFile object using the MemoryStream
                    return new FormFile(memoryStream, 0, memoryStream.Length, "videoFile", Path.GetFileName(filePath))
                    {
                        Headers = new HeaderDictionary(),
                        ContentType = "video/mp4"
                    };
                }
            }
        }
    }
}
