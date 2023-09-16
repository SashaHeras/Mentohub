using Microsoft.AspNetCore.Http;
using Mentohub.Core.Repositories.Repositories;
using Mentohub.Domain.Entities;

namespace Mentohub.Core.Services.Services
{
    public class MediaService
    {
        private CourseService _courseService;
        private AzureService _azureService;

        public MediaService(CourseService courseService, AzureService azureService)
        {
            _courseService = courseService;
            _azureService = azureService;
        }

        /// <summary>
        /// Save media file in project
        /// </summary>
        /// <param name="file"></param>
        /// <param name="courseId"></param>
        /// <returns></returns>
        public async Task<string> SaveMedia(IFormFile? file, int courseId)
        {
            string resPath = String.Empty;
            string oldName = String.Empty;

            if (file != null)
            {
                var course = _courseService.GetCourse(courseId);
                oldName = (file.ContentType == "video/mp4") ? (course.PreviewVideoPath ?? string.Empty) : course.PicturePath;

                if (oldName != null)
                {
                    await _azureService.DeleteFromAzure(oldName);
                }

                resPath = await _azureService.SaveInAsync(file);
            }
            else if (courseId != 0)
            {
                var course = _courseService.GetCourse(courseId);
                resPath = (file.ContentType == "video/mp4") ? (course.PreviewVideoPath ?? string.Empty) : course.PicturePath;
            }

            return resPath;
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
