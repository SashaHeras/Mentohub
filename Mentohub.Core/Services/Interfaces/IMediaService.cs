using Mentohub.Core.Services.Services;
using Mentohub.Domain.Data.DTO;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mentohub.Core.Services.Interfaces
{
    public interface IMediaService
    {
        Task<string> SaveMedia(IFormFile? file, CourseDTO course);

        Task<string> SaveFile(IFormFile file);

        bool DeleteMediaFromProject(IFormFile file);

        Task DeleteFile(string name);
    }
}
