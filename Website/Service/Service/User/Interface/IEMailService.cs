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
using Models.ViewModels.Account;
using Models.ViewModels.Home;
using Models.Class.Email;
using Models.ViewModels.Admin.Email;

namespace Service.UserArea.Interface
{
    public interface IEMailService
    {
        bool SendMail(Email Email);

        void SendMailAsync(Email EMail);

        bool IsEmailAddressValid(string emailAddress);

        bool IsTopLevelDomainValid(string emailAddress);

        bool SendEMailToUser(string UserName, int EMailTypeId);


        bool SendEMailToUser(int UserId, int EMailTypeId);

        EmailTypeLanguage GetEMailTypeLanguage(int EMailTypeId, int LanguageId);

        bool InsertEMailAudit(Email EMail, int AttachmentNumber, int CCUsersNumber);

        DisplayEmailAuditViewModel GetDisplayEmailAuditViewModel(string Pattern, int StartAt, int PageSize);
    }
}
