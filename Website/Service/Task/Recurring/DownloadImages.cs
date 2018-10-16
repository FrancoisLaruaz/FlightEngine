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
using Service.UserArea;

namespace Service.TaskClasses
{
    public class DownloadImages : RecurringJobBase
    {
        AirlineService _AirlineService { get; set; }

        public DownloadImages()
        {
            _AirlineService = new AirlineService();
        }

        public override void Execute(IJobExecutionContext context)
        {
            try
            {
                base.Execute(context);
                bool Result = _AirlineService.DownloadAirlinesImages();
                _taskLogService.UpdateTaskLog(LogId, Result, "N/A");

            }
            catch (Exception e)
            {
                Commons.Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
            }
        }
    }
}