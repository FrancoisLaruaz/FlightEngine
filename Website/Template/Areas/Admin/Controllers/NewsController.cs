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
using Service.Admin.Interface;
using Models.ViewModels.Admin.News;

namespace Website.Areas.Admin.Controllers
{
    public class NewsController : BaseController
    {
        private INewsService _newsService;
        private IScheduledTaskService _scheduledTaskService;


        public NewsController(
            IUserService userService,
            INewsService newsService,
            IScheduledTaskService scheduledTaskService
            ) : base(userService)
        {
            _newsService = newsService;
            _scheduledTaskService = scheduledTaskService;
        }


        [HttpGet]
        public ActionResult Index()
        {
            NewsViewModel Model = new NewsViewModel();

            try
            {
                ViewBag.Title = "[[[News Letter]]]";
                Model = _newsService.GetNewsViewModel();
            }
            catch (Exception e)
            {
                Commons.Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
            }
            return View(Model);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult _PreviewNewsMail(string Title, string Description)
        {
            PreviewNewsMailViewModel model = new PreviewNewsMailViewModel();
            try
            {

                model = _newsService.GetPreviewNewsMailViewModel(Title, Description, UserSession);
            }
            catch (Exception e)
            {
                Commons.Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
                return null;
            }
            return PartialView(model);
        }


        [HttpGet]
        public ActionResult EditNews(int? Id)
        {
            NewsEditViewModel Model = new NewsEditViewModel();

            try
            {
                if (Id != null && Id > 0)
                {
                    ViewBag.Title = "[[[News Letter Edit]]]";
                }
                else
                {
                    ViewBag.Title = "[[[News Letter Creation]]]";
                }
                ViewBag.NewsId = Id;
                Model = _newsService.GetNewsEditViewModel(Id);
            }
            catch (Exception e)
            {
                Commons.Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, "Id = " + Id);
            }
            return View( Model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult EditNews(NewsEditViewModel Model)
        {
            bool _success = false;
            string _Error = "";
            bool _isCreation = false;
            try
            {
                if (User.Identity.IsAuthenticated)
                {
                    if (ModelState.IsValid)
                    {

                        Model.PublishDate = Model.PublishDate.ToUniversalTime();
                        Model.LastModificationUserId = UserSession.UserId;
                        if (Model.TypeId == CommonsConst.NewsType.PublishOnly)
                        {
                            Model.MailSubject = null;
                            Model.TypeUserMailingId = null;
                        }

                        if (Model.Id <= 0)
                        {

                            _isCreation = true;
                            int NewsId = _newsService.CreateNews(Model);
                            Model.Id = NewsId;
                            if (NewsId > 0)
                                _success = true;
                        }
                        else
                        {
                            _success = _newsService.EditNews(Model);
                        }

                        // Scehdule
                        if (_success)
                        {
                            if (!Model.HasScheduledTaskBeenExecuted && Model.ScheduledTaskId.HasValue)
                            {
                                _success = _scheduledTaskService.CancelTaskById(Model.ScheduledTaskId.Value);
                            }

                            if (_success && !Model.HasScheduledTaskBeenExecuted && Model.TypeId != CommonsConst.NewsType.PublishOnly && Model.Active)
                            {
                                if (Model.PublishDate < DateTime.UtcNow)
                                {
                                    Model.PublishDate = DateTime.UtcNow.AddSeconds(5);
                                }

                                _success = _scheduledTaskService.ScheduleNews(Model.Id, Model.PublishDate - DateTime.UtcNow);
                            }
                        }
                    }
                    else
                    {
                        _Error = ModelStateHelper.GetModelErrorsToDisplay(ModelState);
                    }
                }
                else
                {
                    _Error = "[[[You are not logged in.]]]";
                }

                if (!_success && String.IsNullOrWhiteSpace(_Error))
                {
                    _Error = "[[[Error while saving the update.]]]";
                }
            }
            catch (Exception e)
            {
                _success = false;
                Commons.Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, "Id = " + Model.Id);
            }
            return Json(new { Result = _success, Error = _Error, IsCreation = _isCreation });
        }

        /// <summary>
        /// Delete a news by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult DeleteNews(int id)
        {

            bool success = false;
            string err = "";
            int _id = id;
            try
            {
                success = _newsService.DeleteNews(id);
                if (!success)
                {
                    err = "[[[Error while deleting the update.]]]";
                    _id = -1;
                }

            }
            catch (Exception e)
            {
                success = false;
                Commons.Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, "Id = " + id);
            }
            return Json(new { Success = success, Err = err, Id = _id });
        }

        [HttpPost]
        public ActionResult _DisplayPublishedNews(DisplayPublishedNewsViewModel Model)
        {
            try
            {
                Model = _newsService.GetDisplayPublishedNewsViewModel(Model.Pattern, Model.StartAt, Model.PageSize);
            }
            catch (Exception e)
            {
                Commons.Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, "Model.Pattern = " + Model.Pattern);
                return Content("ERROR");
            }
            return PartialView( Model);
        }
    }
}