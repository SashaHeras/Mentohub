using Mentohub.Domain.Data.Entities.CourseEntities;

namespace Mentohub.Core.Services.Interfaces
{
    public interface ICourseViewService : IService
    {
        IQueryable<CourseViews> ViewsByCourse(int CourseID);

        Task<CourseViews> TryAddUserView(int CourseID, string UserID);
    }
}
