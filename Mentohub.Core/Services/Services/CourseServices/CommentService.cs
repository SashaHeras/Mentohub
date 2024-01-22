using Mentohub.Core.Repositories.Interfaces;
using Mentohub.Core.Repositories.Interfaces.CourseInterfaces;
using Mentohub.Domain.Data.DTO;
using Mentohub.Domain.Filters;
using Mentohub.Domain.Helpers;
using Mentohub.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Mentohub.Domain.Data.Entities;
using Mentohub.Core.Services.Interfaces.CourseInterfaces;

namespace Mentohub.Core.Services.Services.CourseServices
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

            if (course == null)
            {
                throw new Exception("Course was not found!");
            }

            var currentUserID = MentoShyfr.Decrypt(data.AuthorId);
            var currentUser = _userRepository.FindByID(currentUserID);
            var comment = _commentRepository.FirstOrDefault(x => x.Id == data.Id);
            if (comment == null)
            {
                comment = new Comment()
                {
                    CourseId = data.CourseId,
                    Text = data.Text,
                    Rating = data.Rating,
                    UserId = currentUserID,
                    DateCreation = DateTime.Now
                };

                if (course.Comments == null)
                {
                    course.Comments = new List<Comment>();
                }

                course.Comments.Add(comment);
            }
            else
            {
                comment.Rating = data.Rating;
                comment.Text = data.Text;
            }


            data.Id = comment.Id;
            data.DateAgo = Helper.GetTimeSinceDate(comment.DateCreation);
            data.UserName = currentUser.LastName == null || currentUser.FirstName == null ?
                            currentUser.Email :
                            currentUser.LastName + currentUser.FirstName.Take(1) + '.';

            course.Rating = course.Comments.Sum(x => x.Rating) / (double)course.Comments.Count();
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
                UserName = UserNameForComment(x.User),
                DateAgo = Helper.GetTimeSinceDate(x.DateCreation),
                ProfileImagePath = x.User.Image ?? "img_avatar.png"
            });

            result = result.Take(data.CommentsCount).ToList();

            return result.ToList();
        }

        private string UserNameForComment(CurrentUser currentUser)
        {
            return currentUser.LastName == null || currentUser.FirstName == null ?
                            currentUser.Email :
                            currentUser.LastName + currentUser.FirstName.Take(1) + '.';
        }
    }
}
