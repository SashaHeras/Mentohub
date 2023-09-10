using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mentohub.Domain.Data.DTO
{
    public class CommentDTO
    {
        public int Id { get; set; }

        public string Text { get; set; }

        public int Rating { get; set; }

        public string DateAgo { get; set; }

        public string UserName { get; set; }

        public string ProfileImagePath { get; set; }
    }
}
