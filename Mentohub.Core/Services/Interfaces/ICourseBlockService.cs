using Mentohub.Domain.Data.DTO.CourseDTOs;
using Mentohub.Domain.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mentohub.Core.Services.Interfaces
{
    public interface ICourseBlockService
    {
        public CourseBlockDTO Edit(CourseBlockDTO data);
    }
}
