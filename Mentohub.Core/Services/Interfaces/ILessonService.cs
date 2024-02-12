using Mentohub.Core.Services.Services;
using Mentohub.Domain.Data.DTO;
using Mentohub.Domain.Entities;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mentohub.Core.Services.Interfaces
{
    public interface ILessonService : IService
    {
        LessonDTO GetLesson(Guid id);

        void UpdateLesson(Lesson newLesson);

        public Task Delete(Guid id);

        LessonDTO GetLessonByCourseItem(int courseItemId);

        Task<LessonDTO> Apply(LessonDTO lesson);

        Task<int> Edit(IFormCollection form, Lesson lesson);
    }
}
