using Mentohub.Core.Repositories.Interfaces.CourseInterfaces;
using Mentohub.Core.Services.Interfaces;

namespace Mentohub.Core.Services.Services
{
    public class CourseSubjectService : ICourseSubjectService
    {
        private readonly ISubjectRepository _subjectRepository;

        public CourseSubjectService(
            ISubjectRepository subjectRepository
            )
        {
            _subjectRepository = subjectRepository;
        }

        public List<KeyValuePair<int, string>> SubjectsList(bool withCourseCount = false)
        {
            var res = _subjectRepository.GetAll()
                                     .ToList()
                                     .Select(x => new KeyValuePair<int, string>(
                                             x.Id, 
                                             withCourseCount == false ? 
                                             x.Name : 
                                             x.Name + " (" + x.Courses.Count.ToString() + ")"
                                         )
                                     )
                                     .ToList();

            return res.OrderBy(x => x.Key).ToList();
        }
    }
}
