using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mentohub.Domain.Data.DTO
{
    public class TaskDTO
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public double Mark { get; set; }

        public int TestId { get; set; }

        public int OrderNumber { get; set; }

        public bool IsFewAnswersCorrect { get; set; }

        public List<AnswerDTO> Answers { get; set; }
    }
}
