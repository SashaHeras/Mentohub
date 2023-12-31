﻿using Mentohub.Core.Context;
using Mentohub.Core.Repositories.Interfaces.CourseInterfaces;
using Mentohub.Domain.Data.Entities.CourseEntities;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Mentohub.Core.Repositories.Repositories.CourseRepositories
{
    public class CourseItemRepository : Repository<CourseItem>, ICourseItemRepository
    {
#pragma warning disable 8603
        private readonly ProjectContext _context;

        public CourseItemRepository(ProjectContext repositoryContext) : base(repositoryContext)
        {
            _context = repositoryContext;
        }

        /// <summary>
        /// Method return CourseItem by id
        /// </summary>
        /// <param name="courseItemId"></param>
        /// <returns></returns>
        public CourseItem GetCourseItemById(int? courseItemId)
        {
            return GetAll().Where(ci => ci.Id == courseItemId)
                           .Include(x => x.Lesson)
                           .Include(x => x.Test)
                           .Include(x => x.Course)
                           .FirstOrDefault();
        }

        /// <summary>
        /// Method return collection of CourseItem with same course id
        /// </summary>
        /// <param name="courseId"></param>
        /// <returns></returns>
        public IQueryable<CourseItem> GetCourseItemsByCourseId(int courseId)
        {
            return GetAll().Where(ci => ci.CourseId == courseId).OrderBy(ci => ci.OrderNumber);
        }

        public CourseItem First(Expression<Func<CourseItem, bool>> expression)
        {
            return GetAll(expression)
                .Include(x => x.Course)
                .Include(x => x.Lesson)
                .Include(x => x.Test)
                .Include(x => x.CourseBlock)
                .FirstOrDefault();
        }
    }
}