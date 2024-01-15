using Mentohub.Core.Services.Services;
using Mentohub.Domain.Data.DTO;
using Mentohub.Domain.Data.DTO.CourseDTOs;
using Mentohub.Domain.Data.Entities.CourseEntities;
using Mentohub.Domain.Filters;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Mentohub.Core.Services.Services.CourseService;

namespace Mentohub.Core.Services.Interfaces
{
    public interface ICourseService
    {
        IQueryable<Course> GetAuthorsCourses(Guid userId);

        List<CourseElementDTO> GetCourseElements(int id);

        List<CommentDTO> GetCourseComments(int courseID, int count = 10);

        Task<CourseDTO> Apply(CourseDTO courseDTO);

        List<CourseDTO> MostFamoustList();

        Task<CourseDTO> ViewCourse(int CourseID, string UserID);

        List<CourseBlockDTO> GetCourseInfoList(int ID);

        List<CourseDTO> List(SearchFilterModel filter, out int totalCount);

        SearchCourseFilterData InitSearchFilterData();
    }
}
