using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mentohub.Domain.Data.DTO.Filters
{
    public class CommentFilter
    {
        public int CourseID { get; set; }

        public int CommentsCount { get; set; } = 4;
    }
}
