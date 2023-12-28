using Mentohub.Core.Infrastructure;
using Mentohub.Domain.Entities;

namespace Mentohub.Core.Repositories.Intefaces
{
    public interface ITestRepository : ISingletoneService, IRepository<Test>
    {
        public Test GetTestByCourseItemId(int courseItemId);

        public Test GetById(int testId);
    }
}
