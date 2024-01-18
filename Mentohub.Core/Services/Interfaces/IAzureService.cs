using Microsoft.AspNetCore.Http;
using Microsoft.WindowsAzure.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mentohub.Core.Services.Interfaces
{
    public interface IAzureService
    {
        Task<string> SaveInAsync(IFormFile file);

        Task<bool> DeleteFromAzure(string name);

        Task<IFormFile> CopyVideoFromBlob(string name);

        IFormFile CreateFormFile(string filePath);
    }
}
