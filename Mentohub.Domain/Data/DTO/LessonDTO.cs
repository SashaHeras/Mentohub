using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Mentohub.Domain.Data.DTO
{
    public class LessonDTO
    {
        public Guid Id { get; set; }

        public string Theme { get; set; }

        public string Description { get; set; }

        public string VideoPath { get; set; }

        public string Body { get; set; }

        public string DateCreation { get; set; }

        public int? CourseItemId { get; set; }

        public int CourseID { get; set; }

        public string CourseName { get; set; }

        public double CourseRating { get; set; }

        public IFormFile VideoFile { get; set; }

        public string AuthorID { get; set; }

        public string AuthorPicture { get; set; }

        public string Authorname { get; set; }

        public int CourseBlockID { get; set; }

        public List<CourseBlockDTO> CourseBlocks { get; set; }
    }
}
