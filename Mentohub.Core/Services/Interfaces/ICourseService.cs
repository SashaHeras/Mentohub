﻿using Mentohub.Core.Services.Services;
using Mentohub.Domain.Data.DTO;
using Mentohub.Domain.Entities;
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

        Task<CourseDTO> Edit(CourseDTO courseDTO);
    }
}
