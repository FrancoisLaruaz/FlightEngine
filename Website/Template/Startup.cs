using Microsoft.Owin;
using Owin;
using System.Transactions;
using System;
using Commons;
using System.Threading.Tasks;
using Service;
using Service.Admin;

[assembly: OwinStartupAttribute(typeof(Template.Startup))]
namespace Template
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            try
            {
                ConfigureAuth(app);
                ScheduledTaskService _scheduledTaskService = new ScheduledTaskService();
                Task.Factory.StartNew(() => _scheduledTaskService.SetTasks());
            }
            catch(Exception e)
            {
                Commons.Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
            }


        }


    }
}


