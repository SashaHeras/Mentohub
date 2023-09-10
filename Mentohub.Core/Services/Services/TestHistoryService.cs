using Mentohub.Core.Repositories.Repositories;
using Mentohub.Domain.Entities;

namespace Mentohub.Core.Services.Services
{
    public class TestHistoryService
    {
        private TestHistoryRepository _testHistoryRepository;

        public TestHistoryService(TestHistoryRepository testHistoryRepository) {
            _testHistoryRepository = testHistoryRepository;
        }

        public async Task<TestHistory> SaveTestHistory(TestHistory history)
        {
            return await _testHistoryRepository.AddAsync(history);
        }
    }
}
