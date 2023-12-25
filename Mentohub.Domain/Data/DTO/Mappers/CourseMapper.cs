﻿using Mentohub.Domain.Data.DTO.Helpers;
using Mentohub.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Mentohub.Domain.Data.DTO.Mappers
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
                Price = course.Price,
                CourseSubjectId = course.CourseSubjectId,
                LastEdittingDate = course.LastEdittingDate,
                Comments = course.Comments.Select(x=>ToDTO(x)).ToList(),
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
    }
}