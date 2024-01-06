using Mentohub.Core.Context;
using Mentohub.Core.Repositories.Intefaces;
using Mentohub.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Mentohub.Core.Repositories.Repositories
{
    public class LessonRepository : Repository<Lesson>, ILessonRepository
    {
        #pragma warning disable 8603
        private readonly ProjectContext _context;

        public LessonRepository(ProjectContext repositoryContext) : base(repositoryContext)
        {
            _context = repositoryContext;
        }

        public Lesson GetLessonByCourseItemId(int courseItemId)
        {
            return GetAll().Where(l => l.CourseItemId == courseItemId).FirstOrDefault();
        }

        public Lesson GetLessonById(Guid id)
        {
            return GetAll().Where(l => l.Id == id).FirstOrDefault();
        }

        public Lesson GetById(Guid id)
        {
            return GetAll()
                .Where(l => l.Id == id)
                .Include(x => x.CourseItem)
                .FirstOrDefault();
        }
    }
}
