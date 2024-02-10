using Mentohub.Core.Context;
using Mentohub.Domain.Data.DTO;
using Mentohub.Domain.Data.Entities.CourseEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mentohub.Core.Services.Interfaces
{
    public interface ICourseItemService : IService
    {
        CourseItem GetCourseItem(int id);

        IQueryable<CourseItem> GetCourseItems(int courseId);

        CourseItemType GetItemTypeByName(string name);

        CourseItemType GetItemType(int id);

        Task<CourseItem> UpdateCourseItem(CourseItem courseItem);

        Task<CourseItem> DeleteCourseItem(int courseItemId, int typeId);

        IQueryable<CourseItem> GetElementsByCourseId(int courseId);
    }
}
