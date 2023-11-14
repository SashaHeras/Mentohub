using Mentohub.Core.Services.Services;
using Mentohub.Domain.Data.DTO;
using Mentohub.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mentohub.Core.Services.Interfaces
{
    public interface ILessonService
    {
        LessonDTO GetLesson(Guid id);

        void UpdateLesson(Lesson newLesson);

        LessonDTO GetLessonByCourseItem(int courseItemId);

        Task<LessonDTO> Edit(LessonDTO lesson);

        void Delete(Guid id);
    }
}
