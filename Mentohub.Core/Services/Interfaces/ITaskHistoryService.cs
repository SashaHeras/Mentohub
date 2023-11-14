﻿using Mentohub.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mentohub.Core.Services.Interfaces
{
    public interface ITaskHistoryService
    {
        Task<List<TaskHistory>> SaveTasksHistory(TestHistory history, List<TaskHistory> taskHistories);
    }
}
