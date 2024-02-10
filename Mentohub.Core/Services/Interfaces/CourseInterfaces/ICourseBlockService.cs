using Mentohub.Domain.Data.DTO.CourseDTOs;

namespace Mentohub.Core.Services.Interfaces
{
    public interface ICourseBlockService : IService
    {
        public CourseBlockDTO Edit(CourseBlockDTO data);
    }
}
