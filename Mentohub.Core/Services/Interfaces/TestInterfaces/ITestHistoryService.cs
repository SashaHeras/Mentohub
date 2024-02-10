using Mentohub.Domain.Entities;

namespace Mentohub.Core.Services.Interfaces
{
    public interface ITestHistoryService : IService
    {
        public Task<TestHistory> SaveTestHistory(TestHistory history);
    }
}
