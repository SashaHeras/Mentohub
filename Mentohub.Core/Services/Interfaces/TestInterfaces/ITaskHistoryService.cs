﻿using Mentohub.Domain.Entities;

namespace Mentohub.Core.Services.Interfaces
{
    public interface ITaskHistoryService : IService
    {
        Task<List<TaskHistory>> SaveTasksHistory(TestHistory history, List<TaskHistory> taskHistories);
    }
}
