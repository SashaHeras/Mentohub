using Microsoft.AspNetCore.Http;
using Microsoft.WindowsAzure.Storage;
using Mentohub.Core.Repositories.Repositories;
using Mentohub.Domain.Entities;

namespace Mentohub.Core.Services.Services
{
    public class AzureService
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
            string uploads = file.ContentType == "video/mp4" ? 
                "C:\\Users\\acsel\\source\\repos\\XMLEdition\\XMLEdition\\wwwroot\\Videos\\" 
                : "C:\\Users\\acsel\\source\\repos\\XMLEdition\\XMLEdition\\wwwroot\\Pictures\\";
            string newName = Guid.NewGuid().ToString().Replace("-", "");

            string filePath = Path.Combine(uploads, file.FileName);
            using (Stream fileStream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(fileStream);
            }

            try
            {
                var storageAccount = CloudStorageAccount.Parse(_connectionString);
                var blobClient = storageAccount.CreateCloudBlobClient();
                var container = blobClient.GetContainerReference("test");

                await container.CreateIfNotExistsAsync();

                var blockBlob = container.GetBlockBlobReference(newName);

                await using (var stream = System.IO.File.OpenRead(uploads + file.FileName))
                {
                    await blockBlob.UploadFromStreamAsync(stream);
                }

                return newName;
            }
            catch (Exception ex)
            {
                // handle exceptions
                return "";
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
