using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Models.Class.Email
{
    public class Email
    {
        public string ToEmail { get; set; }

        public string FromEmail { get; set; }

        public int EMailTypeLanguageId { get; set; }

        public int EMailTypeId { get; set; }

        public int LanguageId { get; set; }

        public string RootPathDefault { get; set; }

        public string EMailTemplate { get; set; }

        public string Subject { get; set; }

        public string Comment { get; set; }

        public string EndMailTemplate { get; set; }

        public List<string> Attachments { get; set; }

        public List<string> CCList { get; set; }

        public List<System.Net.Mail.Attachment> AttachmentsMails { get; set; }

        public int? UserId { get; set; }

        public int? RelatedScheduledTaskId { get; set; }

        public string BasePathFile { get; set; }

        public string AuditGuidId { get; set; }

        public List<Tuple<string,string>> EmailContent { get; set; }
        public Email()
        {
            EmailContent = new List<Tuple<string, string>>();
            Attachments = new List<string>() ;
            CCList = new List<string>();
        }

        public Email(int _UserId,int _EMailTypeId)
        {
            this.UserId = _UserId;
            this.EMailTypeId = _EMailTypeId;
            EmailContent = new List<Tuple<string, string>>();
        }
    }
}
