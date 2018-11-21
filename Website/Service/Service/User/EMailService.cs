using Commons;
using Models.Class;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using Models.ViewModels;
using System.IO;
using CommonsConst;
using Service.UserArea.Interface;
using DataEntities.Repositories;
using DataEntities.Model;
using Models.Class.Email;
using Models.ViewModels.Admin.Email;
using Service.Helpers;

namespace Service.UserArea
{
    public class EMailService : IEMailService
    {

        private string WebsiteURL = ConfigurationManager.AppSettings["Website"];

        private readonly IGenericRepository<ValidTopLevelDomain> _validTopLevelDomainRepo;
        private readonly IGenericRepository<DataEntities.Model.User> _userRepo;
        private readonly IGenericRepository<EmailTypeLanguage> _emailTypeLanguageRepo;
        private IGenericRepository<EmailAudit> _emailAuditRepo;

        public EMailService(IGenericRepository<DataEntities.Model.User> userRepo,
            IGenericRepository<ValidTopLevelDomain> validTopLevelDomainRepo,
            IGenericRepository<EmailTypeLanguage> emailTypeLanguageRepo,
            IGenericRepository<EmailAudit> emailAuditRepo)
        {
            _userRepo = userRepo;
            _validTopLevelDomainRepo = validTopLevelDomainRepo;
            _emailTypeLanguageRepo = emailTypeLanguageRepo;
            _emailAuditRepo = emailAuditRepo;
        }

        public EMailService()
        {
            var context = new TemplateEntities();
            _userRepo = new GenericRepository<DataEntities.Model.User>(context);
            _validTopLevelDomainRepo = new GenericRepository<DataEntities.Model.ValidTopLevelDomain>(context);
            _emailTypeLanguageRepo = new GenericRepository<DataEntities.Model.EmailTypeLanguage>(context);
            _emailAuditRepo = new GenericRepository<DataEntities.Model.EmailAudit>(context);
        }

        public bool IsEmailAddressValid(string emailAddress)
        {
            bool result = false;
            try
            {
                if (Utils.IsValidMail(emailAddress) && IsTopLevelDomainValid(emailAddress))
                {
                    result = true;
                }
            }
            catch (Exception e)
            {
                Commons.Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, " emailAddress = " + emailAddress);
            }
            return result;
        }


        public bool IsTopLevelDomainValid(string emailAddress)
        {
            bool result = false;
            try
            {
                if (!String.IsNullOrWhiteSpace(emailAddress))
                {
                    if (emailAddress.Split('.').Length > 1)
                    {
                        string domain = emailAddress.Split('.')[emailAddress.Split('.').Length - 1].ToLower();
                        result = _validTopLevelDomainRepo.FindAllBy(v => v.Name.ToLower() == domain).Any();
                    }
                }
            }
            catch (Exception e)
            {
                Commons.Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, " emailAddress = " + emailAddress);
            }
            return result;
        }

        /// <summary>
        /// Methode used to send an email
        /// </summary>
        /// <param name="Email"></param>
        /// <returns></returns>
        public bool SendMail(Email Email)
        {
            bool result = false;
            try
            {
                int LanguageId = CommonsConst.Languages.English;
                string LangTag = CommonsConst.Languages.ToString(LanguageId);

                if (String.IsNullOrWhiteSpace(Email.RootPathDefault))
                    Email.RootPathDefault = FileHelper.GetRootPathDefault() + @"\";

                Email.AuditGuidId = Guid.NewGuid().ToString();
                if (Email.EmailContent == null)
                {
                    Email.EmailContent = new List<Tuple<string, string>>();
                }
                DataEntities.Model.User User = null;
                if (Email.UserId != null && Email.UserId.Value > 0)
                {
                    User = _userRepo.Get(Email.UserId.Value);
                }

                if (User != null && User.Id > 0)
                {
                    LangTag = User.Language?.Code ?? CommonsConst.Const.DefaultCulture;
                    LanguageId = User.LanguageId;
                    if (String.IsNullOrWhiteSpace(Email.ToEmail))
                    {
                        Email.ToEmail = User.AspNetUser.Email;
                    }
                    Email.EmailContent.Add(new Tuple<string, string>("#UserFirstName#", User.FirstName));
                    Email.EmailContent.Add(new Tuple<string, string>("#UserFullName#", User.LastName));
                }
                else
                {
                    Email.EmailContent.Add(new Tuple<string, string>("#UserFirstName#", "user"));
                    Email.EmailContent.Add(new Tuple<string, string>("#UserFullName#", "user"));
                }

                if (!String.IsNullOrWhiteSpace(Email.AuditGuidId))
                {
                    Email.EmailContent.Add(new Tuple<string, string>("#WatcherUrl#", Utils.Website + "/Analytics/" + Email.AuditGuidId));
                }
                else
                {
                    Email.EmailContent.Add(new Tuple<string, string>("#WatcherUrl#", FileHelper.GetStorageRoot(DefaultImage.Empty)));
                }
                Email.EmailContent.Add(new Tuple<string, string>("#WebSiteURL#", Utils.Website));
                Email.LanguageId = LanguageId;

                EmailTypeLanguage EMailTypeLanguage = GetEMailTypeLanguage(Email.EMailTypeId, Email.LanguageId);
                if (EMailTypeLanguage == null && Email.LanguageId != Languages.English)
                {
                    EMailTypeLanguage = GetEMailTypeLanguage(Email.EMailTypeId, Languages.English);
                }

                if (EMailTypeLanguage != null && !String.IsNullOrWhiteSpace(Email.ToEmail))
                {
                    if (!Utils.IsProductionWebsite())
                    {
                        Email.EmailContent.Add(new Tuple<string, string>("#RealUserEMail#", "Real mail : " + Email.ToEmail));
                        Email.ToEmail = CommonsConst.Const.EMailDev;
                    }
                    else
                    {
                        Email.EmailContent.Add(new Tuple<string, string>("#RealUserEMail#", "&nbsp;"));
                    }

                    Email.EMailTypeLanguageId = EMailTypeLanguage.Id;
                    Email.EMailTemplate = EMailTypeLanguage.TemplateName;
                    Email.BasePathFile = Email.RootPathDefault + Const.BasePathTemplateEMails.Replace("~/", "\\");

                    if (String.IsNullOrWhiteSpace(Email.Subject))
                        Email.Subject = EMailTypeLanguage.Subject;
                    if (String.IsNullOrWhiteSpace(Email.FromEmail))
                        Email.FromEmail = EMailHelper.MailAdress;
                    Email.AttachmentsMails = new List<System.Net.Mail.Attachment>();
                    if (Email.Attachments != null)
                    {
                        foreach (string file in Email.Attachments)
                        {
                            try
                            {
                                string FileName = FileHelper.GetStorageRoot(file);
                                byte[] BitesTab = FileHelper.GetFileToDownLoad(FileName);
                                if (BitesTab != null)
                                {
                                    Email.AttachmentsMails.Add(new System.Net.Mail.Attachment(new MemoryStream(BitesTab), Path.GetFileName(FileName)));
                                }
                            }
                            catch (Exception e)
                            {
                                result = false;
                                Commons.Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, "file = " + file);
                            }
                        }
                    }
                    if (InsertEMailAudit(Email, Email.Attachments.Count, Email.CCList.Count))
                    {
                        Task.Factory.StartNew(() => SendMailAsync(Email));
                        result = true;
                    }
                }
                else
                {
                    result = false;
                    Commons.Logger.GenerateInfo("No emailtypelanguage found : UserMail = " + Email.ToEmail + " and Language = " + Email.LanguageId + " and EMailTypeId = " + Email.EMailTypeId);
                }

            }
            catch (Exception e)
            {
                result = false;
                Commons.Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, "UserMail = " + Email.ToEmail + " and subject = " + Email.Subject + " and EMailTypeId = " + Email.EMailTypeId);
            }
            return result;
        }

        /// <summary>
        /// Function called in order to send an asynchronous mail
        /// </summary>
        /// <param name="EMail"></param>
        public bool SendMailAsync(Email EMail)
        {
            bool result = false;
            try
            {
                result = EMailHelper.SendMail(EMail);
            }
            catch (Exception e)
            {
                result = false;
                Commons.Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, "EMailTypeId = " + EMail.EMailTypeId + " and emailto =" + EMail.ToEmail);
            }
            return result;
        }


        public bool UpdateEmailWatcher(string EmailGuidId, int EmailWatcherStatusId)
        {
            bool result = false;

            try
            {
                if (!String.IsNullOrWhiteSpace(EmailGuidId))
                {
                    var emailAudit = _emailAuditRepo.FindAllBy(e => e.GuidId == EmailGuidId).FirstOrDefault();
                    if (emailAudit != null && emailAudit.EmailWatcherStatusId != EmailWatcherStatusId)
                    {
                        if (!(emailAudit.EmailWatcherStatusId == EmailWatcherStatus.LinkClicked && EmailWatcherStatusId == EmailWatcherStatus.EmailOpened))
                        {
                            emailAudit.EmailWatcherStatusId = EmailWatcherStatusId;
                        }
                        if ((EmailWatcherStatusId == EmailWatcherStatus.EmailOpened || EmailWatcherStatusId == EmailWatcherStatus.LinkClicked) && emailAudit.EmailOpenedDate == null)
                        {
                            emailAudit.EmailOpenedDate = DateTime.UtcNow;
                        }
                        if ((EmailWatcherStatusId == EmailWatcherStatus.LinkClicked) && emailAudit.EmailLinkClickedDate == null)
                        {
                            emailAudit.EmailLinkClickedDate = DateTime.UtcNow;
                        }
                        emailAudit.EmailSent = true;
                        _emailAuditRepo.Edit(emailAudit);
                        result = _emailAuditRepo.Save();
                    }
                }
            }
            catch (Exception e)
            {
                result = false;
                Commons.Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, "EmailGuidId = " + EmailGuidId + " and EmailWatcherStatusId =" + EmailWatcherStatusId);
            }

            return result;
        }

        /// <summary>
        /// Send a mail to a user
        /// </summary>
        /// <param name="UserName"></param>
        /// <param name="EMailTypeId"></param>
        /// <returns></returns>
        public bool SendEMailToUser(string UserName, int EMailTypeId)
        {
            bool result = false;
            try
            {

                DataEntities.Model.User UserMail = _userRepo.FindAllBy(u => u.AspNetUser.UserName.Trim().ToLower() == UserName.Trim().ToLower()).FirstOrDefault();
                if (UserMail != null)
                {
                    int UserId = UserMail.Id;
                    result = SendEMailToUser(UserId, EMailTypeId);
                }
            }
            catch (Exception e)
            {
                result = false;
                Commons.Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, "EMailTypeId = " + EMailTypeId + " and UserName =" + UserName);
            }
            return result;
        }


        /// <summary>
        /// Send a mail to a user
        /// </summary>
        /// <param name="UserId"></param>
        /// <param name="EMailTypeId"></param>
        /// <returns></returns>
        public bool SendEMailToUser(int UserId, int EMailTypeId)
        {
            bool result = false;
            try
            {
                DataEntities.Model.User UserMail = _userRepo.Get(UserId);
                if (UserMail != null)
                {
                    Email Email = new Email();
                    Email.UserId = UserId;
                    Email.EMailTypeId = EMailTypeId;
                    Email.RootPathDefault = FileHelper.GetRootPathDefault() + @"\";
                    string baseUrl = WebsiteURL + "/" + Languages.ToString(UserMail.LanguageId);

                    List<Tuple<string, string>> EmailContent = new List<Tuple<string, string>>();
                    switch (EMailTypeId)
                    {
                        case CommonsConst.EmailTypes.Forgotpassword:
                            string ResetPasswordUrl = baseUrl + "/ResetPassword/" + UserMail.Id + "/" + Commons.HashHelpers.HashEncode(UserMail.ResetPasswordToken);
                            EmailContent.Add(new Tuple<string, string>("#ResetPasswordUrl_watcher#", ResetPasswordUrl));
                            break;
                        case CommonsConst.EmailTypes.UserWelcome:
                            string ConfirmEmailUrl = baseUrl + "/ConfirmEmail?UserId=" + UserMail.Id + "&Token=" + Commons.HashHelpers.HashEncode(UserMail.EmailConfirmationToken);
                            EmailContent.Add(new Tuple<string, string>("#ConfirmEmailUrl_watcher#", ConfirmEmailUrl));
                            break;
                        case CommonsConst.EmailTypes.ResetPassword:
                            string ChangePasswordUrl = baseUrl + "/MyProfile/password";
                            EmailContent.Add(new Tuple<string, string>("#ChangePasswordUrl_watcher#", ChangePasswordUrl));
                            break;
                    }
                    Email.EmailContent = EmailContent;


                    result = SendMail(Email);
                }
            }
            catch (Exception e)
            {
                result = false;
                Commons.Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, "EMailTypeId = " + EMailTypeId + " and UserId =" + UserId);
            }
            return result;
        }

        /// <summary>
        /// Get the email type with the good language
        /// </summary>
        /// <param name="EMailTypeId"></param>
        /// <param name="LanguageId"></param>
        /// <returns></returns>
        public EmailTypeLanguage GetEMailTypeLanguage(int EMailTypeId, int LanguageId)
        {
            EmailTypeLanguage result = null;
            try
            {
                result = _emailTypeLanguageRepo.FindAllBy(e => e.EMailTypeId == EMailTypeId && LanguageId == e.LanguageId).FirstOrDefault();
            }
            catch (Exception e)
            {
                Commons.Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, "EMailTypeId = " + EMailTypeId.ToString() + " and LanguageId=" + LanguageId.ToString());
            }
            return result;
        }

        /// <summary>
        /// Insert a lign in the database with the result of the mail
        /// </summary>
        /// <param name="EMail"></param>
        /// <param name="AttachmentNumber"></param>
        /// <param name="CCUsersNumber"></param>
        /// <returns></returns>
        public bool InsertEMailAudit(Email EMail, int AttachmentNumber, int CCUsersNumber)
        {

            try
            {

                EmailAudit Audit = new EmailAudit();
                Audit.UserId = EMail.UserId;
                Audit.EMailTypeLanguageId = EMail.EMailTypeLanguageId;
                Audit.EMailFrom = EMail.FromEmail;
                Audit.EMailTo = EMail.ToEmail;
                Audit.Date = DateTime.UtcNow;
                Audit.AttachmentNumber = AttachmentNumber;
                Audit.CCUsersNumber = CCUsersNumber;
                Audit.ScheduledTaskId = EMail.RelatedScheduledTaskId;
                Audit.Comment = EMail.Comment;
                Audit.GuidId = EMail.AuditGuidId;
                Audit.EmailWatcherStatusId = EmailWatcherStatus.EmailNotOpened;
                _emailAuditRepo.Add(Audit);
                return _emailAuditRepo.Save();
            }
            catch (Exception e)
            {
                Commons.Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, "EMailTypeId = " + EMail.EMailTypeId + " and emailto =" + EMail.ToEmail);
            }
            return false;
        }

        public bool SetMailAsSent(string AuditGuidId)
        {
            bool result = false;
            try
            {
                if (!String.IsNullOrWhiteSpace(AuditGuidId))
                {
                    var Audit = _emailAuditRepo.FindAllBy(e => e.GuidId == AuditGuidId).FirstOrDefault();
                    if (Audit != null)
                    {
                        Audit.EmailSent = true;
                        _emailAuditRepo.Edit(Audit);
                        result = _emailAuditRepo.Save();
                    }
                }
            }
            catch (Exception e)
            {
                Commons.Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, "AuditGuidId = " + (AuditGuidId ?? ""));
            }
            return result;
        }

        public DisplayEmailAuditViewModel GetDisplayEmailAuditViewModel(string Pattern, int StartAt, int PageSize)
        {
            DisplayEmailAuditViewModel model = new DisplayEmailAuditViewModel();
            try
            {
                model.Pattern = Pattern;
                model.PageSize = PageSize;
                model.StartAt = StartAt;
                if (Pattern == null)
                    Pattern = "";
                Pattern = Pattern.ToLower().Trim();

                var EmailAudits = new List<EmailAudit>();
                if (String.IsNullOrWhiteSpace(Pattern) && StartAt >= 0 && PageSize >= 0)
                {
                    var FullEmailAuditsList = _emailAuditRepo.List().ToList();
                    model.Count = FullEmailAuditsList.Count;
                    EmailAudits = FullEmailAuditsList.Skip(StartAt).Take(PageSize).OrderByDescending(e => e.Id).ToList();
                }
                else
                {
                    EmailAudits = _emailAuditRepo.List().ToList();
                }

                foreach (var audit in EmailAudits)
                {
                    EMailAuditItem emailAuditItem = new EMailAuditItem();
                    emailAuditItem.Id = audit.Id;
                    emailAuditItem.UserId = audit.UserId;
                    emailAuditItem.NewsId = audit.ScheduledTask?.NewsId;
                    emailAuditItem.ScheduledTaskId = audit.ScheduledTaskId;
                    emailAuditItem.LanguageId = audit.EmailTypeLanguage.LanguageId;
                    emailAuditItem.LanguageName = audit.EmailTypeLanguage.Language.Name;
                    emailAuditItem.EMailTypeId = audit.EmailTypeLanguage.EMailTypeId;
                    emailAuditItem.EMailTypeLanguageId = audit.EMailTypeLanguageId;
                    emailAuditItem.Date = audit.Date.ToLocalTime();
                    emailAuditItem.UserFirstNameDecrypt = audit.User?.FirstName;
                    emailAuditItem.UserLastNameDecrypt = audit.User?.LastName;
                    emailAuditItem.EMailFromDecrypt = audit.EMailFrom;
                    emailAuditItem.EMailToDecrypt = audit.EMailTo;
                    emailAuditItem.CCUsersNumber = audit.CCUsersNumber;
                    emailAuditItem.AttachmentNumber = audit.AttachmentNumber ?? 0;
                    emailAuditItem.NewsTitle = audit.ScheduledTask?.News?.Title;
                    emailAuditItem.Comment = audit.Comment;
                    emailAuditItem.EMailTypeName = audit.EmailTypeLanguage?.EmailType?.Name;

                    model.AuditsList.Add(emailAuditItem);
                }

                if (!String.IsNullOrWhiteSpace(Pattern) && StartAt >= 0 && PageSize >= 0)
                {
                    IEnumerable<EMailAuditItem> resultIEnumerable = model.AuditsList as IEnumerable<EMailAuditItem>;
                    resultIEnumerable = resultIEnumerable.Where(a => (a.Comment != null && a.Comment.ToLower().Contains(Pattern)) || (a.EMailTypeName != null && a.EMailTypeName.ToLower().Contains(Pattern)) || (a.NewsTitle != null && a.NewsTitle.ToLower().Contains(Pattern)) || (a.UserFirstNameDecrypt != null && a.UserFirstNameDecrypt.ToLower().Contains(Pattern)) || a.Id.ToString().Contains(Pattern) || a.EMailTypeName.Contains(Pattern) || (a.EMailToDecrypt != null && a.EMailToDecrypt.ToLower().Contains(Pattern)) || (a.EMailFromDecrypt != null && a.EMailFromDecrypt.ToLower().Contains(Pattern)) || (a.UserLastNameDecrypt != null && a.UserLastNameDecrypt.ToLower().Contains(Pattern)));
                    model.Count = resultIEnumerable.ToList().Count;
                    model.AuditsList = resultIEnumerable.Skip(StartAt).Take(PageSize).OrderByDescending(a => a.Id).ToList();
                }


            }
            catch (Exception e)
            {
                Commons.Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, "Pattern = " + Pattern);
            }
            return model;
        }



    }
}
