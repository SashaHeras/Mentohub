﻿using Mentohub.Domain.Data.DTO;
using Mentohub.Domain.Data.DTO.CourseDTOs;
using Mentohub.Domain.Data.Entities.CourseEntities;
using Mentohub.Domain.Data.Models;
using Mentohub.Domain.Filters;
using static Mentohub.Core.Services.Services.CourseService;

namespace Mentohub.Core.Services.Interfaces
{
    public interface ICourseService : IService
    {
        List<CourseDTO> GetAuthorsCourses(string userId);

        List<CourseBlockDTO> GetCourseElements(int Id, bool justFirstBlock = false);

        public List<CourseElementDTO> GetBlockElements(int ID);

        List<CommentDTO> GetCourseComments(int courseID, int count = 10);

        Task<CourseDTO> Apply(CourseDTO courseDTO);

        List<CourseDTO> MostFamoustList();

        Task<CourseDTO> ViewCourse(int CourseID, string UserID);

        CourseDTO GetCourseInfo(int ID);

        public List<CourseDTO> List(SearchFilterModel filter, out int totalCount);

        public SearchCourseFilterData InitSearchFilterData();

        public List<CourseDTO> GetAuthorsToCourses(string authorID);

        public List<CourseDTO> GetUsersBoughtCourses(string authorID);

        public AuthorInfoDTO GetAuthorInfoDTO(string encriptId);

        public AdditionalListModel GetAdditionalList();

        public TagDTO ApplyCourseTag(int courseID, int tagID, string tagName, string userID);
    }
}
