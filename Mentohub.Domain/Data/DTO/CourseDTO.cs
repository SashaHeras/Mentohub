using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mentohub.Domain.Data.DTO
{
    public class CourseDTO
    {
        public int Id { get; set; }

        public double Rating { get; set; }

        public string Name { get; set; }

        public int DefaultTypeId { get; set; }

        public int DefaultCourseItemId { get; set; }

        public string PicturePath { get; set; }

        public string PreviewVideoPath { get; set; }

        public IFormFile Picture { get; set; }

        public IFormFile PreviewVideo { get; set; }

        public string AuthorId { get; set; }
        public string AuthorName { get; set; }

        public string LoadPictureName { get; set; }

        public string LoadVideoName { get; set; }

        public bool Checked { get; set; }

        public decimal Price { get; set; }

        public int CourseSubjectId { get; set; }

        public DateTime LastEdittingDate { get; set; }

        public List<CourseSubjectDTO> SubjectsList { get; set; }

        public List<CourseElementDTO> CourseElementsList { get; set; }
    }
}
