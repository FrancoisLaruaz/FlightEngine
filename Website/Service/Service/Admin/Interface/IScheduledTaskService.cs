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

namespace Service.Admin.Interface
{
    public interface IScheduledTaskService
    {
        void SetTasks();

        bool IsScheduledTaskActive(string CallBackId);

        int GetNotExecutedTasksNumber();

        int GetActiveScheduledTasksNumber();

        void SetRecurringScheduledTasks();

        bool SetScheduledTasks();

        bool ScheduleNews(int NewsId, TimeSpan callbackDelay, DateTime? CreationDate = null, DateTime? ExpectedExecutionDate = null);

        bool ScheduleEMailUserTask(int UserId, int EMailTypeId, TimeSpan callbackDelay, DateTime? CreationDate = null, DateTime? ExpectedExecutionDate = null);

        List<ScheduledTask> GetScheduledTasksListByUser(int UserId);

        ScheduledTask GetScheduledTaskById(int Id);

        bool SetTaskAsExecuted(string CallBackId);


        bool CancelTaskById(int Id, bool CancelledByUser = false);

        bool CancelTaskByUserId(int UserId);

        bool InsertScheduledTask(ScheduledTask Task);

    }
}
