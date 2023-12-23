using Mentohub.Core.Services.Services;
using Mentohub.Domain.Data.DTO;
using Mentohub.Domain.Data.DTO.Test;
using Mentohub.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mentohub.Core.Services.Interfaces
{
    public interface ITestService
    {
        Test GetTest(int id);

        Test GetTestByCourseItem(int courseItemId);

        void SaveHistory(TestHistory history, List<TaskHistory> taskHistories, List<AnswerHistory> answerHistories);

        Task<Test> CreateNewTest(string testName, int courseId, IQueryable<CourseItem> sameCourseItems);
        
        TestDTO Edit(TestDTO test);

        Task<Test> RenameTest(int testId, string newName);

        Task<PassTestResultDTO> ApplyTestResult(PassTestDTO data);

        TestDTO GetTestModel(int courseItemId);
    }
}
