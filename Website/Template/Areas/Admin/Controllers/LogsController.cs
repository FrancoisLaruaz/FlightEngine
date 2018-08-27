using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using Commons;
using Models.Class;
using Service;
using Models.ViewModels;
using Quartz;
using Quartz.Impl;
using Service.TaskClasses;
using Quartz.Impl.Matchers;
using Models.Class.TaskSchedule;
using Website.Controllers;
using Service.Admin.Interface;
using Service.UserArea.Interface;
using Models.ViewModels.Admin.Logs;

namespace Website.Areas.Admin.Controllers
{
    public class LogsController : BaseController
    {

        private ILogService _logService;

        public LogsController(
            ILogService logService,
            IUserService userService
            ) : base(userService)
        {
            _logService = logService;
        }



        [HttpGet]
        public ActionResult Index()
        {
            try
            {
                ViewBag.Title = "Logs";
            }
            catch (Exception e)
            {
                Commons.Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
            }
            return View();
        }

        [HttpPost]
        public ActionResult _DisplayLogs(DisplayLogsViewModel Model)
        {
            try
            {
                Model = _logService.GetDisplayLogsViewModel(Model.Pattern, Model.StartAt, Model.PageSize);
            }
            catch (Exception e)
            {
                Commons.Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, "Model.Pattern = " + Model.Pattern);
                return Content("ERROR");
            }
            return PartialView(Model);
        }
    }
}