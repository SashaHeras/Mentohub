﻿using Mentohub.Domain.Data.Entities;
using Mentohub.Domain.Data.Entities.CourseEntities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Mentohub.Domain.Entities
{
    public class Comment
    {
        public int Id { get; set; }
        
        public string Text { get; set; }

        public int Rating { get; set; }

        public DateTime DateCreation { get; set; }

        public string UserId { get; set; }

        public int CourseId { get; set; }

        public virtual Course Course { get; set; }

        [ForeignKey("UserId")]
        public virtual CurrentUser User { get; set; }
    }
}