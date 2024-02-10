using Mentohub.Domain.Data.DTO;
using Mentohub.Domain.Data.DTO.Test;
using Mentohub.Domain.Entities;

namespace Mentohub.Core.Services.Interfaces
{
    public interface ITestService : IService
    {
        Test GetTest(int id);

        Test GetTestByCourseItem(int courseItemId);

        void SaveHistory(TestHistory history, List<TaskHistory> taskHistories, List<AnswerHistory> answerHistories);

        TestDTO Apply(TestDTO test);

        Task<PassTestResultDTO> ApplyTestResult(PassTestDTO data);

        TestDTO GetTestModel(int courseItemId);
    }
}
