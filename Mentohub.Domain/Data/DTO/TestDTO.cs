using Mentohub.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mentohub.Domain.Data.DTO
{
    public class TestDTO
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int CourseItemId { get; set; }

        public int CourseID { get; set; }
    }

    public class PassTestDTO : TestDTO
    {
        public List<PassTaskDTO> Tasks { get; set; }
    }

    public class PassTaskDTO
    {
        public int ID { get; set; }

        public List<PassAnswerDTO> Answers { get; set; }
    }

    public class PassAnswerDTO
    {
        public int ID { get; set; }

        public bool Checked { get; set; }
    }
}
