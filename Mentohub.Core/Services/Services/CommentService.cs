using Mentohub.Core.Repositories.Interfaces;
using Mentohub.Core.Services.Interfaces;
using Mentohub.Domain.Data.DTO;
using Mentohub.Domain.Data.DTO.Filters;
using Mentohub.Domain.Entities;
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
        private readonly ICRUD_UserRepository _userRepository;

        public CommentService(
            ICommentRepository commentRepository,
            ICRUD_UserRepository userRepository
            )
        {
            _commentRepository = commentRepository;
            _userRepository = userRepository;
        }

        public CommentDTO Edit(CommentDTO data)
        {
            var comment = _commentRepository.FirstOrDefault(x => x.Id == data.Id);
            if(comment == null)
            {
                comment = new Comment()
                {
                    CourseId = data.CourseId,
                    Text = data.Text,
                    Rating = data.Rating,
                    UserId = Guid.Parse(data.AuthorId),
                    DateCreation = DateTime.Now
                };

                _commentRepository.Add(comment);

                data.Id = comment.Id;
                data.DateAgo = Helper.GetTimeSinceDate(comment.DateCreation);
                data.UserName = "User";
            }
            else
            {
                comment.Rating = data.Rating;
                comment.Text = data.Text;

                _commentRepository.Update(comment);
            }

            return data;
        }

        public List<CommentDTO> List(CommentFilter data)
        {
            var comments = _commentRepository.GetAll()
                                             .Where(x => x.CourseId == data.CourseID)
                                             .ToList();

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
                    DateAgo = Helper.GetTimeSinceDate(com.DateCreation),
                    ProfileImagePath = "img_avatar.png"
                });
            }

            result = result.Take(data.CommentsCount).ToList();

            return result;
        }
    }
}
