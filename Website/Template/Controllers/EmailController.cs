using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using Commons;
using CommonsConst;
using Service.UserArea.Interface;

namespace Website.Controllers
{
    public class EmailController : BaseController
    {

        private readonly IEMailService _emailService;
        public EmailController(
            IUserService userService,
            IEMailService emailService
            ) : base(userService)
        {
            _emailService = emailService;
        }

        public ActionResult Redirect(string EmailAuditGuidId, string Url)
        {
            try
            {
                _emailService.UpdateEmailWatcher(EmailAuditGuidId, EmailWatcherStatus.LinkClicked);
                if (String.IsNullOrWhiteSpace(Url))
                {
                    Url = ConfigurationManager.AppSettings["WebsiteURL"];
                }
            }
            catch (Exception e)
            {
                Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, "EmailAuditGuidId = " + (EmailAuditGuidId ?? "") + " and Url = " + (Url ?? ""));
            }
            return Redirect(Url);
        }

        public FileResult EmailWatcher(string EmailAuditGuidId)
        {
            try
            {

                // Need to add the following hidden image in the mail to work :
                //  <img src ="-WatcherUrl-" alt = "" width = "1" height = "1" border = "0" style = "height:1px!important;width:1px!important;border-width:0!important;margin-top:0!important;margin-bottom:0!important;margin-right:0!important;margin-left:0!important;padding-top:0!important;padding-bottom:0!important;padding-right:0!important;padding-left:0!important" >
                _emailService.UpdateEmailWatcher(EmailAuditGuidId, EmailWatcherStatus.EmailOpened);
            }
            catch (Exception e)
            {
                Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, "EmailAuditGuidId = " + EmailAuditGuidId);
            }
            return File(FileHelper.GetStorageRoot(CommonsConst.DefaultImage.Empty), "image/gif");
        }


    }
}
