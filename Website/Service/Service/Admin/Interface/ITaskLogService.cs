using Commons;

using Models.Class;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models.ViewModels;
using DataEntities.Repositories;
using DataEntities.Model;
using Models.ViewModels.Admin.Tasks;

namespace Service.Admin.Interface
{
    public interface ITaskLogService
    {
        DisplayTasksViewModel GetDisplayTasksViewModel(string Pattern, int StartAt, int PageSize);

        int InsertTaskLog(TaskLog Task);

        bool UpdateTaskLog(int Id, bool Result, string Comment);

    }
}
