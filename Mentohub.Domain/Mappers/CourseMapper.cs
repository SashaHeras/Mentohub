using Mentohub.Domain.Data.DTO;
using Mentohub.Domain.Data.DTO.CourseDTOs;
using Mentohub.Domain.Data.Entities.CourseEntities;
using Mentohub.Domain.Entities;
using Mentohub.Domain.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Mentohub.Domain.Mappers
{
    public static class CourseMapper
    {
        public static CourseDTO ToDTO(Course course)
        {
            return new CourseDTO()
            {
                Id = course.Id,
                AuthorId = course.AuthorId.ToString(),
                Name = course.Name,
                PicturePath = course.PicturePath,
                PreviewVideoPath = course.PreviewVideoPath,
                Checked = course.Checked,
                Rating = course.Rating,
                ShortDescription = course.ShortDescription,
                Description = course.Description,
                Price = course.Price,
                CourseSubjectId = course.CourseSubjectId,
                LanguageId = course.LanguageID ?? 0,
                LastEdittingDate = course.LastEdittingDate,
                Comments = course.Comments.Select(x => ToDTO(x)).ToList(),
                CourseViews = course.CourseViews.Count
            };
        }

        public static CourseDTO ToSimpleCourseDTO(Course course)
        {
            return new CourseDTO()
            {
                Id = course.Id,
                AuthorId = course.AuthorId.ToString(),
                Name = course.Name,
                PicturePath = course.PicturePath,
                PreviewVideoPath = course.PreviewVideoPath,
                Checked = course.Checked,
                Rating = course.Rating,
                ShortDescription = course.ShortDescription,
                Description = course.Description,
                Price = course.Price,
                CourseSubjectId = course.CourseSubjectId,
                LanguageId = course.LanguageID ?? 0,
                LastEdittingDate = new DateTime(
                    course.LastEdittingDate.Year, 
                    course.LastEdittingDate.Month, 
                    course.LastEdittingDate.Day, 
                    course.LastEdittingDate.Hour, 
                    course.LastEdittingDate.Minute, 
                    course.LastEdittingDate.Second, 
                    DateTimeKind.Local),
                CourseViews = course.CourseViews.Count,
                CommentsCount = course.Comments.Count,
                AuthorName = course.Author.LastName + " " + course.Author.FirstName.Remove(1).ToString() + ".",
                Tags = course.CourseTags.Select(x => x.Tag).Select(x => ToDTO(x)).ToList(),
            };
        }

        public static CommentDTO ToDTO(Comment comment)
        {
            return new CommentDTO()
            {
                Id = comment.Id,
                UserName = comment.User?.LastName == null || comment.User?.FirstName == null ?
                                string.Empty : comment.User?.LastName + comment.User?.FirstName.Take(1),
                Text = comment.Text,
                Rating = comment.Rating,
                CourseId = comment.CourseId,
                AuthorId = comment.UserId,
                ProfileImagePath = comment.User?.Image ?? string.Empty,
                DateAgo = Helper.GetTimeSinceDate(comment.DateCreation)
            };
        }

        public static CourseBlockDTO ToDTO(CourseBlock block)
        {
            return new CourseBlockDTO()
            {
                Name = block.Name,
                CourseID = block.CourseID,
                OrderNumber = block.OrderNumber,
                ID = block.ID,
                CourseItems = new List<CourseElementDTO>()
            };
        }

        public static TagDTO ToDTO(Tag tag)
        {
            return new TagDTO()
            {
                Name = tag.Name,
                UserID = tag.UserID,
                Enabled = tag.Enabled,
                Created = tag.Created,
                ID = tag.ID
            };
        }

        public static LanguageDTO ToDTO(CourseLanguage lang)
        {
            return new LanguageDTO()
            {
                Name = lang.Name,
                ID = lang.Id
            };
        }

        public static CourseOverviewDTO ToDTO(CourseOverview overview)
        {
            return new CourseOverviewDTO()
            {
                ID = overview.ID,
                Title = overview.Title,
                CourseID = overview.CourseID,
                Description = overview.Description,
            };
        }
    }
}
