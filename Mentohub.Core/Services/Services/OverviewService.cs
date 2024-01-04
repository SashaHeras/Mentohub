using Mentohub.Core.Repositories.Interfaces.CourseInterfaces;
using Mentohub.Core.Services.Interfaces;
using Mentohub.Domain.Data.DTO.CourseDTOs;
using Mentohub.Domain.Data.Entities.CourseEntities;
using Mentohub.Domain.Mappers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mentohub.Core.Services.Services
{
    public class OverviewService : IOverviewService
    {
        private readonly ICourseOverviewRepository _courseOverviewRepository;
        private readonly ICourseRepository _courseRepository;

        public OverviewService(
            ICourseOverviewRepository courseOverviewRepository,
            ICourseRepository courseRepository
        )
        {
            _courseOverviewRepository = courseOverviewRepository;
            _courseRepository = courseRepository;
        }

        public CourseOverviewDTO Apply(CourseOverviewDTO data)
        {
            var course = _courseRepository.FirstOrDefault(x => x.Id == data.CourseID);
            if(course == null)
            {
                throw new Exception("Unknown course!");
            }

            var overview = _courseOverviewRepository.FindById(data.ID);
            if(overview == null)
            {
                overview = new CourseOverview()
                {
                    Title = data.Title,
                    Description = data.Description,
                    CourseID = data.CourseID
                };

                _courseOverviewRepository.Add(overview);
            }
            else
            {
                overview.Title = data.Title;
                overview.Description = data.Description;

                _courseOverviewRepository.Update(overview);
            }

            return CourseMapper.ToDTO(overview);
        }

        public List<CourseOverviewDTO> GetCourseOverviews(int ID)
        {
            return _courseOverviewRepository.GetCourseOverviews(ID)
                                            .Select(x => CourseMapper.ToDTO(x))
                                            .ToList();
        }
    }
}
