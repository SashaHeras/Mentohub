using Microsoft.AspNetCore.Http;
using Microsoft.WindowsAzure.Storage;
using Mentohub.Core.Repositories.Repositories;
using Mentohub.Domain.Entities;
using Mentohub.Core.Services.Interfaces;
using Microsoft.AspNetCore.Hosting.Server;

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
    }
}
