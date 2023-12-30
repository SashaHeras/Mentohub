using Mentohub.Core.Repositories.Interfaces.CourseInterfaces;
using Mentohub.Core.Services.Interfaces;
using Mentohub.Domain.Data.DTO;
using Mentohub.Domain.Data.DTO.Mappers;
using Mentohub.Domain.Data.Entities;
using Mentohub.Domain.Data.Entities.CourseEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mentohub.Core.Services.Services
{
    public class CourseBlockService : ICourseBlockService
    {
        private readonly ICourseBlockRepository _courseBlockRepository;
        private readonly ICourseRepository _courseRepository;

        public CourseBlockService(
            ICourseBlockRepository courseBlockRepository,
            ICourseRepository courseRepository
        )
        {
            _courseBlockRepository = courseBlockRepository;
            _courseRepository = courseRepository;
        }

        public CourseBlockDTO Edit(CourseBlockDTO data)
        {
            var course = _courseRepository.FirstOrDefault(x => x.Id == data.CourseID);
            if (course == null)
            {
                throw new Exception("Unnown course!");
            }

            int courseBlocksCnt = course.CourseBlocks?.Count() ?? 0;

            var block = _courseBlockRepository.GetById(data.ID);
            if(block == null)
            {
                block = new CourseBlock()
                {
                    CourseID = data.CourseID,
                    Name = data.Name,
                    OrderNumber = courseBlocksCnt + 1
                };

                _courseBlockRepository.Add(block);
            }
            else
            {
                block.OrderNumber = block.OrderNumber;
                block.Name = data.Name;

                _courseBlockRepository.Update(block);
            }

            data = CourseMapper.ToDTO(block);

            return data;
        }
    }
}
