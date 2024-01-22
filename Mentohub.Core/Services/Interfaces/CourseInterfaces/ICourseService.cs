﻿using Mentohub.Domain.Data.DTO;
using Mentohub.Domain.Data.DTO.CourseDTOs;
using Mentohub.Domain.Data.Entities.CourseEntities;
using Mentohub.Domain.Filters;

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

        List<CourseDTO> GetAuthorsToCourses(string authorID);

        AuthorInfoDTO GetAuthorInfoDTO(string encriptId);
    }
}