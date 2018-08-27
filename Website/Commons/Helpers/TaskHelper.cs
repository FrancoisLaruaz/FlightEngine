using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;
using Models;
using Quartz;
using Quartz.Impl;
using Models.Class.TaskSchedule;
using Quartz.Impl.Matchers;
using System.Reflection;
using Quartz.Impl.Triggers;
using DataEntities.Model;

namespace Commons
{
    public static class TaskHelper
    {

        /// <summary>
        /// Get information from a cron expresison
        /// </summary>
        /// <param name="CronExpression"></param>
        /// <returns></returns>
        public static CronInfo GetCronInfo(string CronExpression)
        {
            CronInfo Result = new CronInfo();
            try
            {
                // Infos => https://docs.oracle.com/cd/E12058_01/doc/doc.1014/e12030/cron_expressions.htm
                if (!String.IsNullOrWhiteSpace(CronExpression))
                {
                    CronExpression = CronExpression.Trim();
                    string[] TabCron = CronExpression.Split(' ');

                    if(TabCron.Length==7)
                    {
                        string Second = TabCron[0];
                        string Minute = TabCron[1];
                        string Hour = TabCron[2];
                        string DayOfMonth = TabCron[3];
                        string Month = TabCron[4];
                        string DayOfWeek = TabCron[5];
                        string Year = TabCron[6];

                        DateTime Now = DateTime.UtcNow;
                        DateTime DateSchedule = new DateTime(Now.Year, Now.Month, Now.Day, Convert.ToInt32(Hour), Convert.ToInt32(Minute), Convert.ToInt32(Second)).ToLocalTime();
                   
                        if (DayOfMonth== "*" || DayOfMonth=="?")
                        {
                            Result.Periodicity = CommonsConst.Periodicities.Daily; ;
                            Result.ScheduleInfo = DateSchedule.ToString("HH : mm : ss"); 
                        }
                    }

                }

            }
            catch (Exception e)
            {
                Result = null;
                Commons.Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, "CronExpression = " + CronExpression);
            }
            return Result;
        }


        /// <summary>
        /// Get the information from Quartz
        /// </summary>
        /// <returns></returns>
        public static Models.ViewModels.Admin.Tasks.SchedulerStatusViewModel GetSchedulerInformation()
        {
            Models.ViewModels.Admin.Tasks.SchedulerStatusViewModel Result = new Models.ViewModels.Admin.Tasks.SchedulerStatusViewModel();
            
            try
            {
                IScheduler scheduler = StdSchedulerFactory.GetDefaultScheduler();
                IList<string> jobGroups = scheduler.GetJobGroupNames();
                // IList<string> triggerGroups = scheduler.GetTriggerGroupNames();
                List<object> ListRecurringTasksTypes = Utils.GetPropertiesList(typeof(CommonsConst.TaskLogTypes));
                Result.ScheduledTasksNumberInScheduler = 0;

                foreach (string group in jobGroups)
                {
                    var groupMatcher = GroupMatcher<JobKey>.GroupContains(group);
                    var jobKeys = scheduler.GetJobKeys(groupMatcher);
                    foreach (var jobKey in jobKeys)
                    {
                        var detail = scheduler.GetJobDetail(jobKey);
                        var triggers = scheduler.GetTriggersOfJob(jobKey);
                        foreach (ITrigger trigger in triggers)
                        {
                            
                            // if recurring task
                            if (ListRecurringTasksTypes.Contains(group))
                            {
                                CronTriggerImpl SimpleTrigger = (CronTriggerImpl)trigger;
                                var CronExpression = SimpleTrigger.CronExpressionString;
                                var CronInfo = GetCronInfo(CronExpression);
                                Result.RecurringTaskList.Add(new RecurringTask(group, CronInfo.Periodicity, CronInfo.ScheduleInfo));
                            }
                            else
                            {

                                Result.ScheduledTasksNumberInScheduler = Result.ScheduledTasksNumberInScheduler+1;
                            }

                        }
                    }
                }

            }
            catch (Exception e)
            {
                Result.IsSchedulerActive = false;
                Commons.Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
            }

            return Result;
        }


        /// <summary>
        /// Schedule a recurring Task
        /// </summary>
        /// <param name="Task"></param>
        /// <param name="CallBackDelay"></param>
        /// <param name="Parameters"></param>
        /// <returns></returns>
        public static TaskScheduleResult ScheduleRecurringTask(JobBuilder Task, string CronSchedule, Dictionary<string, object> Parameters = null)
        {
            TaskScheduleResult Result = new TaskScheduleResult();
            Guid Id = Guid.NewGuid();

            IJobDetail job = Task.WithIdentity(Id.ToString(), Task.Build().JobType.Name).Build();
            bool Success = false;
            try
            {
                IScheduler scheduler = StdSchedulerFactory.GetDefaultScheduler();
                scheduler.Start();

                string Group = job.JobType.Name;

                JobKey Key = new JobKey(Id.ToString(), Group);


                TriggerBuilder triggerBuilder = TriggerBuilder.Create().WithIdentity(Id.ToString(), Group).WithCronSchedule(
                     CronSchedule,
                      x => x.InTimeZone(TimeZoneInfo.Utc)
                    );

                if (Parameters != null)
                {
                    foreach (var Param in Parameters)
                    {
                        triggerBuilder = triggerBuilder.UsingJobData(Param.Key, Param.Value.ToString());
                    }
                }
                ITrigger trigger = triggerBuilder.ForJob(job).Build();


                var DatetimeOffset = scheduler.ScheduleJob(job, trigger);
                Success = DatetimeOffset != null ? true : false;
                Result.GroupName = Group;
                Result.Id = Id.ToString();
            }
            catch (Exception e)
            {
                Success = false;
                Commons.Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, "job = " + job.JobType.Name);
            }
            Result.Result = Success;
            return Result;
        }

        /// <summary>
        /// Return a schedule for a cron table (unix style)
        /// </summary>
        /// <param name="Hour"></param>
        /// <param name="Minute"></param>
        /// <returns></returns>
        public static string GetDailyCronSchedule(string Hour,string Minute)
        {
            string result = null;
            try
            {
                result = "0 " + Minute + " " + Hour + " * * ? *";
                
            }
            catch (Exception e)
            {
                result = null;
                Commons.Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, "Hour = " + Hour+ " and Minute = "+ Minute);
            }
            return result;
        }


        /// <summary>
        /// Schedule a task
        /// </summary>
        /// <param name="Task"></param>
        /// <param name="CallBackDelay"></param>
        /// <param name="Parameters"></param>
        /// <returns></returns>
        public static TaskScheduleResult ScheduleTask(JobBuilder Task, TimeSpan CallBackDelay,Dictionary<string,object> Parameters=null)
        {
            TaskScheduleResult Result = new TaskScheduleResult();
            Guid Id = Guid.NewGuid();

            IJobDetail job = Task.WithIdentity(Id.ToString(), Task.Build().JobType.Name).Build();
            bool Success = false;
            try
            {
                IScheduler scheduler = StdSchedulerFactory.GetDefaultScheduler();
                scheduler.Start();

                string Group = job.JobType.Name;

                JobKey Key= new JobKey(Id.ToString(), Group);

                DateTime StartJob = DateTime.UtcNow.Add(CallBackDelay);


                TriggerBuilder triggerBuilder = TriggerBuilder.Create().WithIdentity(Id.ToString(), Group).StartAt(StartJob);

                if (Parameters != null)
                {
                    foreach (var Param in Parameters)
                    {
                        triggerBuilder = triggerBuilder.UsingJobData(Param.Key, Param.Value.ToString());
                    }
                }
                ITrigger trigger = triggerBuilder.ForJob(job).Build();


                var DatetimeOffset=scheduler.ScheduleJob(job, trigger);
                Success = DatetimeOffset!=null ? true:false;
                Result.GroupName = Group;
                Result.Id = Id.ToString();
            }
            catch (Exception e)
            {
                Success = false;
                Commons.Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, "job = " + job.JobType.Name);
            }
            Result.Result = Success;
            return Result;
        }


        /// <summary>
        /// Cancellation of a task
        /// </summary>
        /// <param name="Task"></param>
        /// <returns></returns>
        public static bool CancelTask(ScheduledTask Task)
        {
  
            bool result = true;
            try
            {
                if (Task != null && Task.CancellationDate==null && Task.ExecutionDate==null)
                {
                    IScheduler scheduler = StdSchedulerFactory.GetDefaultScheduler();
                    JobKey Key = new JobKey(Task.CallbackId, Task.GroupName);
                    scheduler.DeleteJob(Key);
                    result = true;
                }
            }
            catch (Exception e)
            {
                result = false;
                Commons.Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, "TaskId = " + Task.Id);
            }
            return result;
        }

    }
}