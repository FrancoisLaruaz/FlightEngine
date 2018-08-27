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

namespace Service.TaskClasses
{
    public class SendNews : JobBase
    {

        public int NewsId { get; set; }

        NewsService _newsService { get; set; }

        public SendNews()
        {
            _newsService = new NewsService();
        }

        public override void Execute(IJobExecutionContext context)
        {
            try
            {
                base.Execute(context);
                if (NeedToBeExectuted && context.MergedJobDataMap.Get("NewsId")!=null)
                {
                    NewsId = Convert.ToInt32(context.MergedJobDataMap.Get("NewsId"));

                    _newsService.SendNews(NewsId);
                }
            }
            catch (Exception e)
            {
                Commons.Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, "NewsId = " + NewsId);
            }
        }
    }
}