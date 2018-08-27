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
using Service.UserArea.Interface;
using Models.ViewModels.Admin.Email;

namespace Website.Areas.Admin.Controllers
{
    public class EmailAuditsController : BaseController
    {

  
        private IEMailService _emailService;

        public EmailAuditsController(
            IUserService userService,
             IEMailService emailService
            ) : base(userService)
        {
            _emailService = emailService;
        }

        [HttpGet]
        public ActionResult Index()
        {
            EmailAuditViewModel model = new EmailAuditViewModel();
            try
            {

                ViewBag.Title = "Email Audits";

            }
            catch (Exception e)
            {
                Commons.Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
            }
            return View(model);
        }

        [HttpPost]
        public ActionResult _DisplayEmailAudits(DisplayEmailAuditViewModel Model)
        {
            try
            {
                Model = _emailService.GetDisplayEmailAuditViewModel(Model.Pattern, Model.StartAt, Model.PageSize);
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