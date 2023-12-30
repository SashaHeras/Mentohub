using Mentohub.Core.Repositories.Interfaces;
using Mentohub.Core.Repositories.Interfaces.CourseInterfaces;
using Mentohub.Core.Services.Interfaces;
using Mentohub.Domain.Data.DTO;
using Mentohub.Domain.Data.DTO.Filters;
using Mentohub.Domain.Data.DTO.Helpers;
using Mentohub.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mentohub.Core.Services.Services
{
    public class CommentService : ICommentService
    {
        private readonly ICommentRepository _commentRepository;
        private readonly ICourseRepository _courseRepository;
        private readonly ICRUD_UserRepository _userRepository;

        public CommentService(
            ICommentRepository commentRepository,
            ICRUD_UserRepository userRepository,
            ICourseRepository courseRepository
            )
        {
            _commentRepository = commentRepository;
            _userRepository = userRepository;
            _courseRepository = courseRepository;
        }

        public CommentDTO Edit(CommentDTO data)
        {
            var course = _courseRepository.GetAll(x => x.Id == data.CourseId)
                                          .Include(x => x.Comments)
                                          .FirstOrDefault();

            if(course == null)
            {
                throw new Exception("Course was not found!");
            }

            var comment = _commentRepository.FirstOrDefault(x => x.Id == data.Id);
            if(comment == null)
            {
                comment = new Comment()
                {
                    CourseId = data.CourseId,
                    Text = data.Text,
                    Rating = data.Rating,
                    UserId = data.AuthorId,
                    DateCreation = DateTime.Now
                };

                if(course.Comments == null)
                {
                    course.Comments = new List<Comment>();
                }

                course.Comments.Add(comment);

                data.Id = comment.Id;
                data.DateAgo = Helper.GetTimeSinceDate(comment.DateCreation);
                data.UserName = "User";
            }
            else
            {
                comment.Rating = data.Rating;
                comment.Text = data.Text;
            }

            course.Rating = (course.Comments.Sum(x => x.Rating) / (double)course.Comments.Count());
            _courseRepository.Update(course);

            return data;
        }

        public List<CommentDTO> List(CommentFilter data)
        {
            var comments = _commentRepository.GetAll()
                                             .Where(x => x.CourseId == data.CourseID)
                                             .ToList();

            var result = comments.Select(x => new CommentDTO()
            {
                Id = x.Id,
                Text = x.Text,
                Rating = x.Rating,
                AuthorId = x.UserId.ToString(),
                CourseId = x.CourseId,
                UserName = "User",
                DateAgo = Helper.GetTimeSinceDate(x.DateCreation),
                ProfileImagePath = "img_avatar.png"
            });

            result = result.Take(data.CommentsCount).ToList();

            return result.ToList();
        }
    }
}
