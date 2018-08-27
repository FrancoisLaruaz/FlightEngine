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
using Service.Admin.Interface;
using Models.ViewModels.Admin.Logs;

namespace Service.Admin
{
    public  class LogService : ILogService
    {
        private readonly IGenericRepository<Log4Net> _logRepo;

        public LogService(IGenericRepository<Log4Net> logRepo)
        {
            _logRepo = logRepo;
        }

        public LogService()
        {
            var context = new TemplateEntities();
            _logRepo = new GenericRepository<Log4Net>(context);
        }



        /// <summary>
        /// Delete old logs
        /// </summary>
        /// <returns></returns>
        public bool DeleteLogs()
        {
            bool result = false;
            try
            {

                DateTime DateToCompare = DateTime.UtcNow.AddMonths(-3);
                List<Log4Net> List = _logRepo.FindAllBy(l => l.Date < DateToCompare).ToList();
                foreach (Log4Net Log in List)
                {
                    _logRepo.Delete(Log.Id);
                }
                result=_logRepo.Save();
            }
            catch (Exception e)
            {
                result = false;
                Commons.Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
            }
            return result;
        }

       
        public  DisplayLogsViewModel GetDisplayLogsViewModel(string Pattern,int StartAt,int  PageSize)
        {
            DisplayLogsViewModel model = new DisplayLogsViewModel();
            try
            {
                model.Pattern = Pattern;
                model.PageSize = PageSize;
                model.StartAt = StartAt;
                if (Pattern == null)
                    Pattern = "";
                Pattern = Pattern.ToLower().Trim();


                if (String.IsNullOrWhiteSpace(Pattern) && StartAt >= 0 && PageSize >= 0)
                {
                    var FullLogsList = _logRepo.List().ToList();
                    model.Count = FullLogsList.Count;
                    model.LogsList = FullLogsList.OrderByDescending(e => e.Id).Skip(StartAt).Take(PageSize).ToList();
                }
                else
                {
                    model.LogsList = _logRepo.List().OrderByDescending(e => e.Id).ToList();
                }

                if (!String.IsNullOrWhiteSpace(Pattern) && StartAt >= 0 && PageSize >= 0)
                {
                    IEnumerable<Log4Net> resultIEnumerable = model.LogsList as IEnumerable<Log4Net>;
                    resultIEnumerable = resultIEnumerable.Where(a => (a.UserLogin != null && a.UserLogin!="" && a.UserLogin.ToLower().Contains(Pattern)) || a.Id.ToString().Contains(Pattern) || a.Level.ToLower().Contains(Pattern) || (a.Exception != null && a.Exception.ToLower().Contains(Pattern)) || (a.Logger != null && a.Logger.ToLower().Contains(Pattern)) || (a.Message != null && a.Message.ToLower().Contains(Pattern) || (a.Thread != null && a.Message.Contains(Pattern))));
                    model.Count = resultIEnumerable.ToList().Count;
                    model.LogsList = resultIEnumerable.OrderByDescending(a => a.Id).Skip(StartAt).Take(PageSize).ToList();
                }
            }
            catch (Exception e)
            {
                Commons.Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType,"Pattern = " + Pattern);
            }
            return model;
        }
    }
}
