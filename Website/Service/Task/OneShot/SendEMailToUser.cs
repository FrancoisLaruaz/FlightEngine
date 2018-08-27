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
using Service.UserArea;

namespace Service.TaskClasses
{
    public class SendEMailToUser : JobBase
    {

        public int UserId { get; set; }

        public int EmailType { get; set; }

        EMailService _emailService { get; set; }

        public SendEMailToUser()
        {
            _emailService = new EMailService();
        }

        public override void Execute(IJobExecutionContext context)
        {
            try
            {
                base.Execute(context);
                if (NeedToBeExectuted)
                {
                    UserId = Convert.ToInt32(context.MergedJobDataMap.Get("UserId"));
                    EmailType = Convert.ToInt32(context.MergedJobDataMap.Get("EMailTypeId"));


                    _emailService.SendEMailToUser(UserId, EmailType);
                }
            }
            catch (Exception e)
            {
                Commons.Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, "UserId = "+ UserId+ " and EmailType = "+ EmailType);
            }
        }
    }
}