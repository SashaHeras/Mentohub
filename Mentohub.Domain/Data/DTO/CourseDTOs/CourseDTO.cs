using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mentohub.Domain.Data.DTO.CourseDTOs
{
    public class CourseDTO
    {
        public int Id { get; set; }

        public double Rating { get; set; }

        public string Name { get; set; }

        public int DefaultTypeId { get; set; }

        public int CourseViews { get; set; }

        public int DefaultCourseItemId { get; set; }

        public string ShortDescription { get; set; }

        public string Description { get; set; }

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

        public List<CourseSubjectDTO> SubjectsList { get; set; }

        public int LanguageId { get; set; }

        public List<LanguageDTO> LanguageList { get; set; }

        public int CourseLevelId { get; set; }

        public List<KeyValuePair<int,string>> CourseLevelList { get; set; }

        public DateTime LastEdittingDate { get; set; }

        public List<CommentDTO> Comments { get; set; }

        public List<CourseElementDTO> CourseElementsList { get; set; }


    }
}
