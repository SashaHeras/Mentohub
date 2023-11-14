﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mentohub.Domain.Data.DTO
{
    public class AnswerDTO
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int TaskId { get; set; }

        public bool IsChecked { get; set; }
    }
}
