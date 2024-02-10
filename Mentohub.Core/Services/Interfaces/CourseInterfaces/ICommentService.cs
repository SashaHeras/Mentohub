using Mentohub.Domain.Data.DTO;
using Mentohub.Domain.Filters;

namespace Mentohub.Core.Services.Interfaces
{
    public interface ICommentService : IService
    {
        public CommentDTO Edit(CommentDTO data);

        public List<CommentDTO> List(CommentFilter data);
    }
}
