using Mentohub.Core.Infrastructure;
using Mentohub.Domain.Entities;

namespace Mentohub.Core.Repositories.Intefaces
{
    public interface ILessonRepository : ISingletoneService, IRepository<Lesson>
    {
        public Lesson GetLessonById(Guid id);

        public Lesson GetLessonByCourseItemId(int id);
    }
}
