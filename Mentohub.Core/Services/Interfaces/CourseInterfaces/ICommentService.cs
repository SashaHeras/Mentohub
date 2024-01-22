using Mentohub.Domain.Data.DTO;
using Mentohub.Domain.Filters;

namespace Mentohub.Core.Services.Interfaces
{
    public interface ICommentService
    {
        public CommentDTO Edit(CommentDTO data);

        public List<CommentDTO> List(CommentFilter data);
    }
}
