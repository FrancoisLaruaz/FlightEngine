using Commons;
using Models.Class;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models.ViewModels;
using DataEntities.Repositories;
using CommonsConst;
using Service.Admin.Interface;
using Service.UserArea.Interface;
using Service.UserArea;
using Models.ViewModels.Admin.News;
using Models.Class.Email;
using DataEntities.Model;
using Models.Class.News;
using Service.Helpers;

namespace Service.Admin
{
    public class NewsService : INewsService
    {
        private readonly IEMailService _emailService;
        private readonly ICategoryService _categoryService;
        private readonly IScheduledTaskService _scheduledTaskService;
        private readonly IGenericRepository<DataEntities.Model.News> _newsRepo;
        private readonly IGenericRepository<DataEntities.Model.User> _userRepo;

        public NewsService(IEMailService emailService, ICategoryService categoryService,
             IGenericRepository<DataEntities.Model.News> newsRepo,
             IGenericRepository<DataEntities.Model.User> userRepo,
             IScheduledTaskService scheduledTaskService)
        {
            _emailService = emailService;
            _categoryService = categoryService;
            _newsRepo = newsRepo;
            _scheduledTaskService = scheduledTaskService;
            _userRepo = userRepo;
        }

        public NewsService()
        {
            _emailService = new EMailService();
            var context = new TemplateEntities();
            _userRepo = new GenericRepository<User>(context);
            _newsRepo = new GenericRepository<News>(context);
        }



        /// <summary>
        /// Return all the news not published
        /// </summary>
        /// <returns></returns>
        public List<NewsItem> GetNotPublishedNewsList()
        {
            List<NewsItem> result = new List<NewsItem>();
            try
            {
                var NewsList = _newsRepo.FindAllBy(n => n.PublishDate >= DateTime.UtcNow).ToList();
                foreach (var news in NewsList)
                {
                    NewsItem newsItem = TransformNewsIntoNewsItem(news);
                    if (news != null)
                    {
                        result.Add(newsItem);
                    }
                }
            }
            catch (Exception e)
            {
                result = null;
                Commons.Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
            }
            return result;
        }


        public List<NewsItem> GetPublishedNewsList()
        {
            List<NewsItem> result = new List<NewsItem>();
            try
            {
                var NewsList = _newsRepo.FindAllBy(n => n.PublishDate <DateTime.UtcNow).ToList();
                foreach (var news in NewsList)
                {
                    NewsItem newsItem = TransformNewsIntoNewsItem(news);
                    if (news != null)
                    {
                        result.Add(newsItem);
                    }
                }
            }
            catch (Exception e)
            {
                result = null;
                Commons.Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
            }
            return result;
        }

        /// <summary>
        /// Creationf of a news
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public int CreateNews(NewsEditViewModel model)
        {
            int InsertedId = -1;
            try
            {

                News news = new News();
                news.PublishDate = model.PublishDate;
                news.Title = model.NewsTitle;
                news.Description = model.NewsDescription;
                news.MailSubject = model.MailSubject;
                news.TypeId = model.TypeId;
                news.TypeUserMailingId = model.TypeUserMailingId;

                news.ModificationDate = DateTime.UtcNow;
                news.Active = model.Active;
                news.LastModificationUserId = model.LastModificationUserId;
                news.CreationDate = DateTime.UtcNow;

                _newsRepo.Add(news);
                if (_newsRepo.Save())
                {
                    InsertedId = news.Id;
                }
            }
            catch (Exception e)
            {
                InsertedId = -1;
                Commons.Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
            }
            return InsertedId;
        }


        /// <summary>
        /// Edit an existing news
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool EditNews(NewsEditViewModel model)
        {
            bool Result = false;
            try
            {
                News news = _newsRepo.Get(model.Id);
                if (news != null)
                {
                    news.PublishDate = model.PublishDate;
                    news.Title = model.NewsTitle;
                    news.Description = model.NewsDescription;
                    news.MailSubject = model.MailSubject;
                    news.TypeId = model.TypeId;
                    news.TypeUserMailingId = model.TypeUserMailingId;
                    news.Active = model.Active;
                    news.LastModificationUserId = model.LastModificationUserId;
                    news.ModificationDate = DateTime.UtcNow;

                    _newsRepo.Edit(news);
                    Result = _newsRepo.Save();

                }

            }
            catch (Exception e)
            {
                Result = false;
                Commons.Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
            }
            return Result;
        }

        /// <summary>
        /// Get the viewmodel for the News form
        /// </summary>
        /// <param name="NewsId"></param>
        /// <returns></returns>
        public NewsEditViewModel GetNewsEditViewModel(int? NewsId)
        {
            NewsEditViewModel model = new NewsEditViewModel();
            try
            {
                // Creation
                if (NewsId == null || NewsId <= 0)
                {
                    model.Id = -1;
                    model.Active = false;
                    model.PublishDate = DateTime.UtcNow.ToLocalTime();
                }
                // Modification
                else
                {
                    News RealNews = GetNewsById(NewsId.Value);
                    model.Id = NewsId.Value;
                    if (RealNews != null)
                    {
                        model.NewsTitle = RealNews.Title;
                        model.NewsDescription = RealNews.Description;
                        model.MailSubject = RealNews.MailSubject;
                        model.TypeUserMailingId = RealNews.TypeUserMailingId;
                        model.TypeId = RealNews.TypeId;
                        model.Active = RealNews.Active;
                        model.Id = RealNews.Id;
                        model.PublishDate = RealNews.PublishDate.ToLocalTime();
                        model.ScheduledTaskId = RealNews.ScheduledTasks?.FirstOrDefault()?.Id;
                        model.HasScheduledTaskBeenExecuted = RealNews.ScheduledTasks?.FirstOrDefault()?.ExecutionDate < DateTime.UtcNow ? true : false;
                    }
                }


                model.NewsTypeList = _categoryService.GetSelectionList(CommonsConst.CategoryTypes.NewsType);
                model.TypeUserMailingList = _categoryService.GetSelectionList(CommonsConst.CategoryTypes.TypeUserMailing);
            }
            catch (Exception e)
            {
                Commons.Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
            }
            return model;
        }

        public NewsViewModel GetNewsViewModel()
        {
            NewsViewModel model = new NewsViewModel();
            try
            {
                model.NotPublishedNews = GetNotPublishedNewsList();
            }
            catch (Exception e)
            {
                Commons.Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
            }
            return model;
        }

        /// <summary>
        /// Delete a news
        /// </summary>
        /// <param name="NewsId"></param>
        /// <returns></returns>
        public bool DeleteNews(int NewsId)
        {
            bool result = false;
            try
            {
                News news = GetNewsById(NewsId);
                if (news != null && news.Id>0)
                {
                    int? ScheduledTaskId = news.ScheduledTasks?.FirstOrDefault()?.Id;
                    if (ScheduledTaskId != null)
                        _scheduledTaskService.CancelTaskById(ScheduledTaskId.Value);
                    List<Tuple<string, object>> Parameters = new List<Tuple<string, object>>();
                    Parameters.Add(new Tuple<string, object>("@NewsId", NewsId));
                    result = _newsRepo.ExecuteStoredProcedure("DeleteNewsById", Parameters);
                }

            }
            catch (Exception e)
            {
                result = false;
                Commons.Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, "NewsId = " + NewsId);
            }
            return result;
        }

        /// <summary>
        /// Send a news to the users
        /// </summary>
        /// <param name="NewsId"></param>
        /// <returns></returns>
        public bool SendNews(int NewsId)
        {
            bool result = false;
            try
            {
                News news = GetNewsById(NewsId);
                if (news != null)
                {
                    List<int> MailingList = GetNewsMailList(news.TypeUserMailingId.Value);

                    foreach (int UserId in MailingList)
                    {

                        Email Email = new Email();
                        Email.UserId = UserId;
                        Email.EMailTypeId = CommonsConst.EmailTypes.News;
                        Email.RootPathDefault = FileHelper.GetRootPathDefault() + @"\";
                        List<Tuple<string, string>> EmailContent = new List<Tuple<string, string>>();
                        EmailContent.Add(new Tuple<string, string>("#Description#", news.Description));
                        EmailContent.Add(new Tuple<string, string>("#Title#", news.Title));

                        Email.Subject = news.MailSubject;
                        Email.EmailContent = EmailContent;
                        Email.RelatedScheduledTaskId = news.ScheduledTasks?.FirstOrDefault()?.Id;

                        result = _emailService.SendMail(Email);

                    }
                }
            }
            catch (Exception e)
            {
                result = false;
                Commons.Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, "NewsId = " + NewsId);
            }
            return result;
        }

        public PreviewNewsMailViewModel GetPreviewNewsMailViewModel(string Title, string Description, UserSession user)
        {
            PreviewNewsMailViewModel model = new PreviewNewsMailViewModel();
            try
            {
                if (Title == null)
                    Title = "";
                if (Description == null)
                    Description = "";

                List<Tuple<string, string>> GenericEmailContent = EMailHelper.GetGenericEmailContent();
                GenericEmailContent.Add(new Tuple<string, string>("#UserFirstName#", user.FirstName));
                GenericEmailContent.Add(new Tuple<string, string>("#UserFullName#", user.UserFullName));
                GenericEmailContent.Add(new Tuple<string, string>("#RealUserEMail#", ""));
                GenericEmailContent.Add(new Tuple<string, string>("#WebSiteURL#", Utils.Website));

                string BasePathFile = FileHelper.GetRootPathDefault() + @"\" + Const.BasePathTemplateEMails.Replace("~/", "\\");
                string PathHeaderOnServer = BasePathFile + "\\_HeaderMail.html";
                string PathFooterOnServer = BasePathFile + "\\_FooterMail.html";
                string PathEndMailOnServer = BasePathFile + "\\_EndMail_en.html";
                string PathTemplateOnServer = BasePathFile + "\\news_en.html";
                string headerTemplate = System.IO.File.ReadAllText(PathHeaderOnServer);
                string bodyTemplate = System.IO.File.ReadAllText(PathTemplateOnServer);
                string footerTemplate = System.IO.File.ReadAllText(PathFooterOnServer);
                string endMailTemplate = System.IO.File.ReadAllText(PathEndMailOnServer);
                model.Body = headerTemplate + bodyTemplate + endMailTemplate + footerTemplate;

                foreach (var content in GenericEmailContent)
                {
                    model.Body = model.Body.Replace(content.Item1, content.Item2);
                }
                model.Body = model.Body.Replace("#Title#", Title).Replace("#Description#", Description);

            }
            catch (Exception e)
            {
                Commons.Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, "Title = " + Title);
            }

            return model;
        }


        public NewsItem TransformNewsIntoNewsItem(News news)
        {
            NewsItem model = new NewsItem();
            try
            {
                if (news != null)
                {

                    model.Id = news.Id;
                    ScheduledTask task = news.ScheduledTasks?.FirstOrDefault();
                    model.HasScheduledTaskBeenExecuted= news.ScheduledTasks.Any() ? (news.ScheduledTasks.FirstOrDefault().ExecutionDate == null ? false : true) : false;
                    model.LastModificationUserFullNameDecrypt = news.User?.FirstName + " " +news.User?.LastName;
                    model.LastModificationUserId = news.User?.Id;
                    model.Active = news.Active;
                    model.CreationDate = news.CreationDate.ToLocalTime();
                    model.Description = news.Description;
                    model.Title = news.Title;
                    model.MailSubject = news.MailSubject;
                    model.ModificationDate = news.ModificationDate.ToLocalTime();
                    model.PublishDate = news.PublishDate.ToLocalTime();
                    model.Title = news.Title;
                    model.TypeId = news.TypeId;
                    model.TypeName = news.Type?.Name;
                    model.TypeUserMailingId = news.TypeUserMailingId;
                    model.TypeUserMailingName = news.TypeUserMailing?.Name;

                    if (task!=null)
                    {
                        model.HasScheduledTaskBeenExecuted = task.ExecutionDate == null ? false : true;
                        model.MailSentNumber = task.EmailAudits.Count();
                        model.ScheduledTaskId = task.Id;
                    }
                    else
                    {
                        model.MailSentNumber = 0;
                    }

                }
                else
                {
                    return null;
                }
            }
            catch(Exception e)
            {
                Commons.Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, "news.Id = " + news.Id);
            }
            return model;
        }


        public DisplayPublishedNewsViewModel GetDisplayPublishedNewsViewModel(string Pattern, int StartAt, int PageSize)
        {
            DisplayPublishedNewsViewModel model = new DisplayPublishedNewsViewModel();
            try
            {
                model.Pattern = Pattern;
                model.PageSize = PageSize;
                model.StartAt = StartAt;
                if (Pattern == null)
                    Pattern = "";
                Pattern = Pattern.ToLower();

                var News = new List<News>();
                if (String.IsNullOrWhiteSpace(Pattern) && StartAt >= 0 && PageSize >= 0)
                {
                    var FulNewsList = _newsRepo.FindAllBy(n => n.PublishDate<=DateTime.UtcNow).ToList();
                    model.Count = FulNewsList.Count;
                    News = FulNewsList.OrderByDescending(e => e.Id).Skip(StartAt).Take(PageSize).ToList();
                }
                else
                {
                    News = _newsRepo.FindAllBy(n => n.PublishDate <= DateTime.UtcNow).OrderByDescending(e => e.Id).ToList();
                }

                foreach (var news in News)
                {
                    NewsItem newsItem = TransformNewsIntoNewsItem(news);
                    if (news != null)
                    {
                        model.NewsList.Add(newsItem);
                    }
                }

                if (!String.IsNullOrWhiteSpace(Pattern) && StartAt >= 0 && PageSize >= 0)
                {
                    IEnumerable<NewsItem> resultIEnumerable = model.NewsList as IEnumerable<NewsItem>;
                    resultIEnumerable = resultIEnumerable.Where(a => (a.Title != null && a.Title.Contains(Pattern)) || (a.MailSubject != null && a.MailSubject.Contains(Pattern)) || (a.Description != null && a.Description.Contains(Pattern)) || a.Id.ToString().Contains(Pattern) || a.TypeName.Contains(Pattern) || (a.TypeUserMailingName != null && a.TypeUserMailingName.Contains(Pattern)) || (a.PublishDate != null && a.PublishDate.ToString().Contains(Pattern)));
                    model.Count = resultIEnumerable.ToList().Count;
                    model.NewsList = resultIEnumerable.OrderByDescending(a => a.Id).Skip(StartAt).Take(PageSize).ToList();
                }


            }
            catch (Exception e)
            {
                Commons.Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, "Pattern = " + Pattern);
            }
            return model;
        }

        /// <summary>
        /// Get the list of the user to send the mail
        /// </summary>
        /// <param name="TypeUserMailingId"></param>
        /// <returns></returns>
        public  List<int> GetNewsMailList(int TypeUserMailingId)
        {
            List<int> result = new List<int>();
            try
            {


                switch (TypeUserMailingId)
                {
                    case CommonsConst.TypeUserMailing.AllUsers:
                        result = _userRepo.List().Select(u => u.Id).ToList();
                        break;
                    case CommonsConst.TypeUserMailing.ConfirmedUsers:
                        result = _userRepo.FindAllBy(u => u.AspNetUser.EmailConfirmed.HasValue && u.AspNetUser.EmailConfirmed.Value).Select(u => u.Id).ToList();
                        break;
                }


            }
            catch (Exception e)
            {
                Commons.Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, "TypeUserMailingId = " + TypeUserMailingId);
            }
            return result;
        }

        /// <summary>
        /// Get a news By Id
        /// </summary>
        /// <param name="NewsId"></param>
        /// <returns></returns>
        public News GetNewsById(int NewsId)
        {
            News result = null;
            try
            {
                result = _newsRepo.Get(NewsId);
            }
            catch (Exception e)
            {
                result = null;
                Commons.Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, "NewsId = " + NewsId);
            }
            return result;
        }

    }
}
