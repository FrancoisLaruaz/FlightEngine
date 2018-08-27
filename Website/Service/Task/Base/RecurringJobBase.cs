using Quartz;
using System;
using System.Net;
using System.Net.Mail;
using Commons;

using Models.Class;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Service.Admin;
using DataEntities.Model;

namespace Service.TaskClasses
{
    public class RecurringJobBase : IJob
    {
        public int LogId { get; set; }


        protected TaskLogService _taskLogService { get; set; }

        public RecurringJobBase()
        {
            _taskLogService = new TaskLogService();
        }



        public virtual void Execute(IJobExecutionContext context)
        {
            try
            {
                TaskLog Log = new TaskLog();
                Log.CallbackId= context.JobDetail.Key.Name;
                Log.GroupName = context.JobDetail.Key.Group;
                Log.StartDate = DateTime.UtcNow;
                LogId = _taskLogService.InsertTaskLog(Log);
            }
            catch (Exception e)
            {
                Commons.Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, "Id = " + context.JobDetail.Key.Name);
            }
        }
    }
}