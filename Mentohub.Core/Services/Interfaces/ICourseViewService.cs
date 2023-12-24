using Mentohub.Domain.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mentohub.Core.Services.Interfaces
{
    public interface ICourseViewService
    {
        IQueryable<CourseViews> ViewsByCourse(int CourseID);

        Task<CourseViews> TryAddUserView(int CourseID, string UserID);
    }
}
