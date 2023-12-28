using Mentohub.Core.Context;
using Mentohub.Core.Repositories.Intefaces;
using Mentohub.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Mentohub.Core.Repositories.Repositories
{
    public class AnswerRepository : Repository<TaskAnswer>, IAnswerRepository
    {
        #pragma warning disable 8603
        private readonly ProjectContext _context;

        public AnswerRepository(ProjectContext repositoryContext) : base(repositoryContext)
        {
            _context = repositoryContext;
        }

        public TaskAnswer GetById(int id)
        {
            return _context.TaskAnswers
                           .Where(ta => ta.Id == id)   
                           .Include(x => x.TestTask)
                           .Include(x => x.AnswerHistory)
                           .FirstOrDefault();
        }

        public bool DeleteAnswer(TaskAnswer answer)
        {
            try
            {
                _context.TaskAnswers.Remove(answer);
                _context.SaveChanges();
            }
            catch(Exception ex)
            {
                return false;
            }

            return true;
        }

        public IQueryable<TaskAnswer> GetAnswersByTaskId(int taskId)
        {
            return _context.TaskAnswers.Where(ta => ta.TaskId == taskId).AsQueryable<TaskAnswer>();
        }

        public int GetCountOfCorrectAnswers(int taskId)
        {
            return _context.TaskAnswers.Count(ta => ta.TaskId == taskId && ta.IsCorrect);
        }

        public TaskAnswer GetAnswerByIdAndTask(int id, int taskId) {
            return _context.TaskAnswers
                    .Where(ta => ta.TaskId == taskId && ta.Id == id)
                    .First();
        }
    }
}
