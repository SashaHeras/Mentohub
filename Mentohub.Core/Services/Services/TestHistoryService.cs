using Mentohub.Core.Repositories.Intefaces;
using Mentohub.Core.Services.Interfaces;
using Mentohub.Domain.Entities;

namespace Mentohub.Core.Services.Services
{
    public class TestHistoryService : ITestHistoryService
    {
        private ITestHistoryRepository _testHistoryRepository;

        public TestHistoryService(ITestHistoryRepository testHistoryRepository) {
            _testHistoryRepository = testHistoryRepository;
        }

        public async Task<TestHistory> SaveTestHistory(TestHistory history)
        {
            return await _testHistoryRepository.AddAsync(history);
        }
    }
}
