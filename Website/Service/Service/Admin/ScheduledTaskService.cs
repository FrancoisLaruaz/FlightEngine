using Commons;
using Models.Class;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using Quartz;
using Quartz.Impl;
using Service.TaskClasses;
using Models.Class.TaskSchedule;
using Models.ViewModels.Admin.News;
using Models.Class.Email;
using DataEntities.Model;
using Service.Admin.Interface;
using DataEntities.Repositories;


namespace Service.Admin
{
    public  class ScheduledTaskService : IScheduledTaskService
    {
        private  string WebsiteURL = ConfigurationManager.AppSettings["Website"];


        private readonly IGenericRepository<ScheduledTask> _scheduledTaskRepo;

        public ScheduledTaskService(IGenericRepository<ScheduledTask> scheduledTaskRepo)
        {
            _scheduledTaskRepo = scheduledTaskRepo;
        }

        public ScheduledTaskService()
        {
            var context = new TemplateEntities();
            _scheduledTaskRepo = new GenericRepository<ScheduledTask>(context);
        }

        /// <summary>
        /// Set the tasks when the application starts
        /// </summary>
        public  void SetTasks()
        {
            try
            {
                if(!Utils.IsLocalhost())
                    Logger.GenerateInfo("SetTasks CALLED");
                var SchedulerInfo = TaskHelper.GetSchedulerInformation();
                if (SchedulerInfo.RecurringTaskList.Count==0 && SchedulerInfo.ScheduledTasksNumberInScheduler==0)
                {
                    if (!Utils.IsLocalhost())
                        Logger.GenerateInfo("SetTasks RESET");
                    SetRecurringScheduledTasks();
                    SetScheduledTasks();
                }
            }
            catch (Exception e)
            {
                Commons.Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
            }
        }

        /// <summary>
        /// Check if a task need to be executed or no
        /// </summary>
        /// <param name="CallBackId"></param>
        /// <returns></returns>
        public  bool IsScheduledTaskActive(string CallBackId)
        {
            bool result = false;
            try
            {
                result = _scheduledTaskRepo.FindAllBy(c => c.CallbackId == CallBackId && c.CancellationDate == null && c.ExecutionDate == null).Any();
            }
            catch (Exception e)
            {
                result = false;
                Commons.Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
            }
            return result;
        }


        public  int GetNotExecutedTasksNumber()
        {
            int result = 0;
            try
            {
                result = _scheduledTaskRepo.FindAllBy(c => c.ExpectedExecutionDate < DateTime.UtcNow && c.CancellationDate == null && c.ExecutionDate == null).Count(); ;
            }
            catch (Exception e)
            {
                result = 0;
                Commons.Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
            }
            return result;
        }


        public  int GetActiveScheduledTasksNumber()
        {
            int result = 0;
            try
            {
                result = _scheduledTaskRepo.FindAllBy(c => c.ExpectedExecutionDate >= DateTime.UtcNow && c.CancellationDate == null && c.ExecutionDate == null).Count(); 
            }
            catch (Exception e)
            {
                result = 0;
                Commons.Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
            }
            return result;
        }

        /// <summary>
        /// Set the recurring daily task
        /// </summary>
        public  void SetRecurringScheduledTasks()
        {
            try
            {
                TaskHelper.ScheduleRecurringTask(JobBuilder.Create<DeleteLogs>(), TaskHelper.GetDailyCronSchedule("03", "33"));
                TaskHelper.ScheduleRecurringTask(JobBuilder.Create<DeleteUploadedFile>(), TaskHelper.GetDailyCronSchedule("16", "33"));
                TaskHelper.ScheduleRecurringTask(JobBuilder.Create<ConvertCurrency>(), TaskHelper.GetDailyCronSchedule("17", "10"));
                TaskHelper.ScheduleRecurringTask(JobBuilder.Create<DownloadImages>(), TaskHelper.GetDailyCronSchedule("15", "55"));
                TaskHelper.ScheduleRecurringTask(JobBuilder.Create<WeatherHistory>(), TaskHelper.GetDailyCronSchedule("15", "55"));
            }
            catch (Exception e)
            {
                Commons.Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
            }
        }

        /// <summary>
        /// Set the scheduled task when the application starts
        /// </summary>
        public  bool SetScheduledTasks()
        {
            bool result = true;
            try
            {
                Random random = new Random();
                var ListTaskToSchedule = _scheduledTaskRepo.FindAllBy(c => c.ExpectedExecutionDate >= DateTime.UtcNow && c.CancellationDate == null && c.ExecutionDate == null).ToList();
                foreach (var Task in ListTaskToSchedule)
                {
                    if (CancelTaskById(Task.Id))
                    {

                        if (Task.ExpectedExecutionDate <= DateTime.UtcNow)
                        {
                            Task.ExpectedExecutionDate = DateTime.UtcNow.AddSeconds(random.Next(5, 500));
                        }
                        TimeSpan Delay = Task.ExpectedExecutionDate - DateTime.UtcNow;

                        if (Task.GroupName == CommonsConst.ScheduledTaskTypes.SendEMailToUser)
                        {
                            result = result && ScheduleEMailUserTask(Task.UserId.Value, Task.EmailTypeId.Value, Delay, Task.CreationDate, Task.ExpectedExecutionDate);
                        }
                        else if(Task.GroupName== CommonsConst.ScheduledTaskTypes.SendNews)
                        {
                            result = result && ScheduleNews(Task.NewsId.Value, Delay, Task.CreationDate, Task.ExpectedExecutionDate);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                result = false;
                Commons.Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
            }
            return result;
        }


        /// <summary>
        /// Schedule a news mail
        /// </summary>
        /// <param name="NewsId"></param>
        /// <param name="callbackDelay"></param>
        /// <param name="CreationDate"></param>
        /// <param name="ExpectedExecutionDate"></param>
        /// <returns></returns>
        public  bool ScheduleNews(int NewsId, TimeSpan callbackDelay, DateTime? CreationDate = null, DateTime? ExpectedExecutionDate = null)
        {
            bool Result = false;
            try
            {

                if (CreationDate == null)
                {
                    CreationDate = DateTime.UtcNow;
                    ExpectedExecutionDate = CreationDate.Value.Add(callbackDelay);
                }


                Dictionary<string, object> Parameters = new Dictionary<string, object>();
                Parameters.Add("NewsId", NewsId);
                TaskScheduleResult ResultSchedule = TaskHelper.ScheduleTask(JobBuilder.Create<SendNews>(), callbackDelay, Parameters);

                if (ResultSchedule != null && ResultSchedule.Result)
                {
                    ScheduledTask Task = new ScheduledTask();
                    Task.NewsId = NewsId;
                    Task.CreationDate = CreationDate.Value;
                    Task.ExpectedExecutionDate = ExpectedExecutionDate.Value;
                    Task.CallbackId = ResultSchedule.Id;
                    Task.GroupName = ResultSchedule.GroupName;
                    Result = InsertScheduledTask(Task);
                }

            }
            catch (Exception e)
            {
                Result = false;
                Commons.Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, "NewsId = " + NewsId);
            }
            return Result;
        }

        /// <summary>
        /// Schedule a mail for a user
        /// </summary>
        /// <param name="UserId"></param>
        /// <param name="EMailTypeId"></param>
        /// <returns></returns>
        public  bool ScheduleEMailUserTask(int UserId, int EMailTypeId, TimeSpan callbackDelay, DateTime? CreationDate = null, DateTime? ExpectedExecutionDate = null)
        {
            bool Result = false;
            try
            {

                if (CreationDate == null)
                {
                    CreationDate = DateTime.UtcNow;
                    ExpectedExecutionDate = CreationDate.Value.Add(callbackDelay);
                }


                Dictionary<string, object> Parameters = new Dictionary<string, object>();
                Parameters.Add("UserId", UserId);
                Parameters.Add("EMailTypeId", EMailTypeId);
                TaskScheduleResult ResultSchedule = TaskHelper.ScheduleTask(JobBuilder.Create<SendEMailToUser>(), callbackDelay, Parameters);

                if (ResultSchedule != null && ResultSchedule.Result)
                {
                    ScheduledTask Task = new ScheduledTask();
                    Task.UserId = UserId;
                    Task.EmailTypeId = EMailTypeId;
                    Task.CreationDate = CreationDate.Value;
                    Task.ExpectedExecutionDate = ExpectedExecutionDate.Value;
                    Task.CallbackId = ResultSchedule.Id;
                    Task.GroupName = ResultSchedule.GroupName;
                    Result = InsertScheduledTask(Task);
                }

            }
            catch (Exception e)
            {
                Result = false;
                Commons.Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, "UserId = " + UserId.ToString() + " and EMailTypeId = " + EMailTypeId);
            }
            return Result;
        }

        public  List<ScheduledTask> GetScheduledTasksListByUser(int UserId)
        {
            List<ScheduledTask> Result = new List<ScheduledTask>();
            try
            {
                Result = _scheduledTaskRepo.FindAllBy(c => c.UserId== UserId).ToList(); 
            }
            catch (Exception e)
            {
                Commons.Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, "UserId = " + UserId.ToString());
            }
            return Result;
        }



        /// <summary>
        /// Return the scheduled task
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public  ScheduledTask GetScheduledTaskById(int Id)
        {
            ScheduledTask Result = null;
            try
            {
                Result = _scheduledTaskRepo.Get(Id);
            }
            catch (Exception e)
            {
                Commons.Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, "Id = " + Id.ToString());
            }
            return Result;
        }



        /// <summary>
        /// Set a task as executed
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public  bool SetTaskAsExecuted(string CallBackId)
        {
            bool Result = false;
            try
            {
                if (!string.IsNullOrWhiteSpace(CallBackId))
                {
                    ScheduledTask task = _scheduledTaskRepo.FindAllBy(s => s.CallbackId == CallBackId).FirstOrDefault();
                    if (task != null)
                    {
                        task.ExecutionDate = DateTime.UtcNow;
                        _scheduledTaskRepo.Edit(task);
                        Result = _scheduledTaskRepo.Save();
                    }
                }
            }
            catch (Exception e)
            {
                Result = false;
                Commons.Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, "CallBackId = " + CallBackId);
            }
            return Result;
        }

        public bool DeleteScheduledTaskById(int SchduledTaskId)
        {
            bool result = false;
            try
            {
                var TaskToDelete = _scheduledTaskRepo.Get(SchduledTaskId);
                if (TaskToDelete != null)
                {
                    List<Tuple<string, object>> Parameters = new List<Tuple<string, object>>();
                    Parameters.Add(new Tuple<string, object>("@ScheduledTaskId", TaskToDelete.Id));
                    result = _scheduledTaskRepo.ExecuteStoredProcedure("DeleteScheduledTaskById", Parameters);
                }
            }
            catch (Exception e)
            {
                result = false;
                Commons.Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, "SchduledTaskId = " + SchduledTaskId);
            }
            return result;
        }


        /// <summary>
        ///  Cancel a Task by Id
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="CancelledByUser"></param>
        /// <returns></returns>
        public  bool CancelTaskById(int Id, bool CancelledByUser = false)
        {
            bool Result = false;
            try
            {
                ScheduledTask Task = GetScheduledTaskById(Id);
                Result = Commons.TaskHelper.CancelTask(Task);

                if (Result)
                {
                    if (CancelledByUser)
                    {
                        ScheduledTask task = _scheduledTaskRepo.Get(Id);
                        if(task!=null)
                        {
                            task.CancellationDate=DateTime.UtcNow;
                            _scheduledTaskRepo.Edit(task);
                            Result=_scheduledTaskRepo.Save();
                        }

                    }
                    else
                    {
                        Result = DeleteScheduledTaskById(Id);
                    }
                }

            }
            catch (Exception e)
            {
                Result = false;
                Commons.Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, "Id = " + Id.ToString());
            }
            return Result;
        }


        /// <summary>
        /// Cancel wll the scheduled task of a user
        /// </summary>
        /// <param name="UserId"></param>
        /// <returns></returns>
        public  bool CancelTaskByUserId(int UserId)
        {
            bool Result = false;
            try
            {
                List<ScheduledTask> TaskList = GetScheduledTasksListByUser(UserId);
                foreach (var Task in TaskList)
                {
                    Result = Result && CancelTaskById(Task.Id);
                }
            }
            catch (Exception e)
            {
                Result = false;
                Commons.Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, "UserId = " + UserId.ToString());
            }
            return Result;
        }


        /// <summary>
        /// Insertion of a row in scheduledtask table
        /// </summary>
        /// <param name="Task"></param>
        /// <returns></returns>
        public  bool InsertScheduledTask(ScheduledTask Task)
        {
            bool Result = false;
            try
            {
                ScheduledTask task = new ScheduledTask();
                task.CallbackId= Task.CallbackId;
                task.GroupName = Task.GroupName;
                task.UserId = Task.UserId;
                task.ExpectedExecutionDate = Task.ExpectedExecutionDate;
                task.EmailTypeId = Task.EmailTypeId;
                task.CreationDate = Task.CreationDate;
                task.NewsId = Task.NewsId;
                _scheduledTaskRepo.Add(task);
                Result = _scheduledTaskRepo.Save();
                
            }
            catch (Exception e)
            {
                Result = false;
                Commons.Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
            }
            return Result;
        }

    }
}
