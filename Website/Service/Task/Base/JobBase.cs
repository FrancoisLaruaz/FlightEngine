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
using Service.Admin.Interface;
using Service.Admin;

namespace Service.TaskClasses
{
    public class JobBase : IJob
    {

        public Boolean NeedToBeExectuted { get; set; }

        public IScheduledTaskService _scheduledTaskService { get; set; }

        public JobBase()
        {
            NeedToBeExectuted = false;
            _scheduledTaskService = new ScheduledTaskService();
        }



        public virtual void Execute(IJobExecutionContext context)
        {
            try
            {
                string CallBackId = context.JobDetail.Key.Name;
                NeedToBeExectuted = _scheduledTaskService.IsScheduledTaskActive(CallBackId);
                if (NeedToBeExectuted)
                {
                    _scheduledTaskService.SetTaskAsExecuted(CallBackId);
                }
                else
                {
                    Logger.GenerateInfo("Task not active triggered :  "+ CallBackId);
                }
            }
            catch (Exception e)
            {
                Commons.Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, "Id = " + context.JobDetail.Key.Name);
            }
        }
    }
}