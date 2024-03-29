﻿using System.ComponentModel;
using System.Text.Json;
using System.Threading;
using Azure.Core;
using Mentohub.Core.Context;
using Mentohub.Core.Repositories.Intefaces;
using Mentohub.Core.Repositories.Interfaces;
using Mentohub.Core.Repositories.Repositories;
using Mentohub.Core.Services.Interfaces;
using Mentohub.Domain.Data.DTO;
using Mentohub.Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using static System.Net.Mime.MediaTypeNames;

namespace Mentohub.Core.Services.Services
{
    public class CourseService : ICourseService
    {
        private readonly ProjectContext _context;

        private readonly ICourseRepository _courseRepository;
        private readonly ICourseItemRepository _courseItemRepository;
        private readonly ICourseTypeRepository _courseTypeRepository;
        private readonly ILessonRepository _lessonRepository;
        private readonly ICommentRepository _commentRepository;

        private readonly IMediaService _mediaService;
        private readonly ICourseItemService _courseItemService;

        public CourseService(
            ProjectContext context,
            ICourseRepository courseRepository,
            ICourseItemRepository courseItemRepository,
            ICourseTypeRepository courseTypeRepository,
            ICommentRepository commentRepository,
            ILessonRepository lessonRepository,
            ICourseItemService courseItemService,
            IMediaService mediaService)
        {
            this._context = context;
            _courseRepository = courseRepository;
            _courseItemRepository = courseItemRepository;
            _courseTypeRepository = courseTypeRepository;
            _lessonRepository = lessonRepository;
            _courseItemService = courseItemService;
            _mediaService = mediaService;
            _commentRepository = commentRepository;
        }

        public async Task<CourseDTO> Edit(CourseDTO courseDTO)
        {
            var currentCourse = _courseRepository.FirstOrDefault(x => x.Id == courseDTO.Id);
            if(currentCourse == null)
            {
                currentCourse = new Course()
                {
                    Name = courseDTO.Name,
                    AuthorId = Guid.Parse(courseDTO.AuthorId),
                    Checked = false,
                    Rating = 0.00,
                    Price = courseDTO.Price,
                    CourseSubjectId = (int)courseDTO.CourseSubjectId,
                    LastEdittingDate = DateTime.Now
                };

                try
                {
                    currentCourse.PicturePath = await _mediaService.SaveFile(courseDTO.Picture);
                }
                catch(Exception ex)
                {
                    throw ex;
                }

                try
                {
                    currentCourse.PreviewVideoPath = await _mediaService.SaveFile(courseDTO.PreviewVideo);
                }
                catch (Exception ex)
                {
                    throw ex;
                }

                currentCourse.LoadPictureName = courseDTO.Picture.FileName;
                currentCourse.LoadVideoName = courseDTO.PreviewVideo.FileName;

                _courseRepository.Add(currentCourse);
            }
            else
            {
                currentCourse.Name = courseDTO.Name;
                currentCourse.Checked = courseDTO.Checked;
                currentCourse.Rating = courseDTO.Rating;
                currentCourse.Price = courseDTO.Price;
                currentCourse.LastEdittingDate = DateTime.Now;
                currentCourse.CourseSubjectId = courseDTO.CourseSubjectId;
                            
                if (currentCourse.LoadPictureName == null || courseDTO.Picture.FileName != currentCourse.LoadPictureName)
                {
                    if (currentCourse.LoadPictureName == null)
                    {
                        await _mediaService.DeleteFile(currentCourse.PicturePath);
                    }

                    currentCourse.PicturePath = await _mediaService.SaveFile(courseDTO.Picture);
                    currentCourse.LoadPictureName = courseDTO.Picture.FileName;
                }

                if (currentCourse.LoadVideoName == null || courseDTO.PreviewVideo.FileName != currentCourse.LoadVideoName)
                {
                    if (currentCourse.LoadVideoName == null)
                    {
                        await _mediaService.DeleteFile(currentCourse.PreviewVideoPath);
                    }

                    currentCourse.PreviewVideoPath = await _mediaService.SaveFile(courseDTO.PreviewVideo);
                    currentCourse.LoadVideoName = courseDTO.PreviewVideo.FileName;
                }

                _courseRepository.Update(currentCourse);
            }

            courseDTO.Id = currentCourse.Id;
            courseDTO.LoadVideoName = currentCourse.LoadVideoName;
            courseDTO.LoadPictureName = currentCourse.LoadPictureName;
            courseDTO.LastEdittingDate = currentCourse.LastEdittingDate;

            return courseDTO;
        }

        /// <summary>
        /// Execute stored procedure to generate list of course elements
        /// </summary>
        /// <param name="id"></param>
        /// <returns>
        /// Elements list in Json format
        /// </returns>
        public List<CourseElementDTO> GetCourseElements(int id)
        {
            var elementsJson = _courseRepository.GetCourseElementsList(id.ToString());
            var entityList = JsonSerializer.Deserialize<List<CourseElement>>(elementsJson);
            var result = new List<CourseElementDTO>();
            if (entityList != null)
            {
                foreach (var e in entityList)
                {
                    result.Add(new CourseElementDTO()
                    {
                        CourseItemId = e.CourseItemId,
                        TypeId = e.TypeId,
                        CourseId = e.CourseId,
                        DateCreation = e.DateCreation,
                        OrderNumber = e.OrderNumber,
                        ElementName = e.ElementName
                    });
                }
            }

            return result;
        }

        public List<CommentDTO> GetCourseComments(int courseID, int count = 10)
        {
            var comments = _commentRepository.GetAll().Where(x => x.CourseId == courseID).ToList();
            var result = new List<CommentDTO>();
            foreach (var com in comments)
            {
                result.Add(new CommentDTO()
                {
                    Text = com.Text,
                    Rating = com.Rating,
                    AuthorId = com.UserId.ToString(),
                    Id = com.Id,
                    CourseId = com.CourseId,
                    UserName = "User",
                    DateAgo = GetTimeSinceDate(com.DateCreation),
                    ProfileImagePath = "img_avatar.png"
                });
            }

            result = result.Take(count).ToList();

            return result;
        }
        private static string GetTimeSinceDate(DateTime date)
        {
            DateTime currentDate = DateTime.Now;
            TimeSpan timeDifference = currentDate - date;

            int years = currentDate.Year - date.Year;
            int months = currentDate.Month - date.Month;
            int days = currentDate.Day - date.Day;
            int hours = currentDate.Hour - date.Hour;

            if (years > 0)
            {
                return $"{years} year{(years > 1 ? "s" : "")} ago";
            }
            else if (months > 0)
            {
                return $"{months} month{(months > 1 ? "s" : "")} ago";
            }
            else if (days > 0)
            {
                return $"{days} day{(days > 1 ? "s" : "")} ago";
            }
            else
            {
                if (hours == 0)
                {
                    return $"right now";
                }
                return $"{hours} hour{(hours > 1 ? "s" : "")} ago";
            }
        }

        public IQueryable<Course> GetAuthorsCourses(Guid userId)
        {
            throw new NotImplementedException();
        }

        public List<CourseDTO> GetUserCourses(Guid userId) {
            var result = new List<CourseDTO>();
            var courses = _courseRepository.GetAllAuthorsCourses(userId).ToList();
            foreach(var course in courses)
            {
                result.Add(new CourseDTO()
                {
                    Id = course.Id,
                    Name = course.Name,
                    Price = course.Price,
                    Rating = course.Rating,
                    PicturePath = course.PicturePath,
                    AuthorName = "User",
                });
            }

            return result;
        }
    }
}
