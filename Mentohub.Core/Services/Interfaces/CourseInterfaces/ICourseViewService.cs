using Mentohub.Domain.Data.Entities.CourseEntities;

namespace Mentohub.Core.Services.Interfaces
{
    public interface ICourseViewService
    {
        IQueryable<CourseViews> ViewsByCourse(int CourseID);

        Task<CourseViews> TryAddUserView(int CourseID, string UserID);
    }
}
