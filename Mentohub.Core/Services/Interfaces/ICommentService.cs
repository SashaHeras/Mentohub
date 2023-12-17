using Mentohub.Domain.Data.DTO;
using Mentohub.Domain.Data.DTO.Filters;
using Mentohub.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mentohub.Core.Services.Interfaces
{
    public interface ICommentService
    {
        public CommentDTO Edit(CommentDTO data);

        public List<CommentDTO> List(CommentFilter data);
    }
}
