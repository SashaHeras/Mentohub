using Microsoft.AspNetCore.Http;
using Mentohub.Core.Repositories.Repositories;
using Mentohub.Domain.Entities;
using Mentohub.Core.Services.Interfaces;
using Mentohub.Domain.Data.DTO.CourseDTOs;

namespace Mentohub.Core.Services.Services
{
    public class MediaService : IMediaService
    {
        private IAzureService _azureService;

        public MediaService(IAzureService azureService)
        {
            _azureService = azureService;
        }

        /// <summary>
        /// Save media file in project
        /// </summary>
        /// <param name="file"></param>
        /// <param name="courseId"></param>
        /// <returns></returns>
        public async Task<string> SaveMedia(IFormFile? file, CourseDTO course)
        {
            string resPath = String.Empty;
            string oldName = String.Empty;

            if (file != null)
            {
                oldName = (file.ContentType == "video/mp4") ? (course.PreviewVideoPath ?? string.Empty) : course.PicturePath;

                if (oldName != null)
                {
                    await _azureService.DeleteFromAzure(oldName);
                }

                resPath = await _azureService.SaveInAsync(file);
            }
            else if (course != null)
            {
                resPath = (file.ContentType == "video/mp4") ? (course.PreviewVideoPath ?? string.Empty) : course.PicturePath;
            }

            return resPath;
        }

        public async Task<string> SaveFile(IFormFile file)
        {
            string resPath = await _azureService.SaveInAsync(file);
            return resPath;
        }

        public async Task DeleteFile(string name)
        {
            await _azureService.DeleteFromAzure(name);
        }

        /// <summary>
        /// Method delete file from project folders Videos and Pictures after uploading file on Azure
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public bool DeleteMediaFromProject(IFormFile file)
        {
            string uploads = file.ContentType == "video/mp4" ?
               "C:\\Users\\acsel\\source\\repos\\XMLEdition\\XMLEdition\\wwwroot\\Videos\\"
               : "C:\\Users\\acsel\\source\\repos\\XMLEdition\\XMLEdition\\wwwroot\\Pictures\\";
            string fullPath = uploads + file.FileName;

            try
            {
                if (File.Exists(fullPath))
                {
                    // Delete the file
                    File.Delete(fullPath);
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

            return true;
        }
    }
}
