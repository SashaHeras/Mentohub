using Mentohub.Core.Repositories.Intefaces;
using Mentohub.Core.Services.Interfaces;
using Mentohub.Core.Services.Services;
using Mentohub.Domain.Data.DTO;
using Mentohub.Domain.Entities;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Mentohub.Tests.ServiceTests
{
    public class TestServicesTests
    {
        Mock<ITaskRepository> _taskRepositoryMock = new Mock<ITaskRepository>();
        Mock<IAnswerRepository> _answerRepositoryMock = new Mock<IAnswerRepository>();
        Mock<IAnswerService> _answerServiceMock = new Mock<IAnswerService>();
        private readonly TaskService _taskService;

        public TestServicesTests()
        {
                _taskService = new TaskService(
                _taskRepositoryMock.Object,
                _answerRepositoryMock.Object,
                _answerServiceMock.Object);
        }

        [Fact]
        public void Edit_Task_Returns_Updated_TaskDTO()
        {
            var data = new TaskDTO
            {
                Id = 1,
                TestId = 1,
                Name = "Test Task",
                Mark = 10,
                OrderNumber = 1,
                IsFewAnswersCorrect = true,
                Answers = new List<AnswerDTO>
            {
                new AnswerDTO { Id = 1, Name = "Answer 1", IsChecked = true },
                new AnswerDTO { Id = 2, Name = "Answer 2", IsChecked = false }
            }
            };

            var existingTask = new TestTask
            {
                Id = 1,
                TestId = 1,
                Name = "Old Test Task",
                Mark = 5,
                OrderNumber = 1,
                IsFewAnswersCorrect = false
            };

            _taskRepositoryMock.Setup(repo => repo.GetById(data.Id)).Returns(existingTask);
            _taskRepositoryMock.Setup(repo => repo.Update(It.IsAny<TestTask>())).Returns(existingTask);

            var updatedAnswers = new List<TaskAnswer>();
            _answerRepositoryMock.Setup(repo => repo.FirstOrDefault(It.IsAny<Expression<Func<TaskAnswer, bool>>>()))
                .Returns((Expression<Func<TaskAnswer, bool>> predicate) => updatedAnswers.FirstOrDefault(predicate.Compile()));

            _answerRepositoryMock.Setup(repo => repo.Update(It.IsAny<TaskAnswer>()))
                .Callback((TaskAnswer updatedAnswer) => updatedAnswers.Add(updatedAnswer));

            _answerRepositoryMock.Setup(repo => repo.Add(It.IsAny<TaskAnswer>()))
                .Callback((TaskAnswer newAnswer) => updatedAnswers.Add(newAnswer));


            var result = _taskService.Edit(data);


            Assert.NotNull(result);
            Assert.Equal(data.Id, result.Id);
            Assert.Equal(data.OrderNumber, result.OrderNumber);
            Assert.Equal(data.Name, result.Name);
            Assert.Equal(data.Mark, result.Mark);
            Assert.Equal(data.IsFewAnswersCorrect, result.IsFewAnswersCorrect);
            Assert.Equal(data.Answers.Count, result.Answers.Count);
        }
    }
}
