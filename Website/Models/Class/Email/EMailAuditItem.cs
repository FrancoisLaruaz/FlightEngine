using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Models.Class.Email
{
    public class EMailAuditItem
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public string EMailFrom { get; set; }
        public string EMailTo { get; set; }
        public int? UserId { get; set; }

        public int AttachmentNumber { get; set; }
        public int CCUsersNumber { get; set; }
        public int EMailTypeLanguageId { get; set; }

        public int? ScheduledTaskId { get; set; }

        public string Comment { get; set; }

        public int LanguageId { get; set; }

        public int EMailTypeId { get; set; }

        public string LanguageName { get; set; }

        public string UserFirstNameDecrypt { get; set; }
        public string UserLastNameDecrypt { get; set; }

        public string EMailFromDecrypt { get; set; }
        public string EMailTypeName { get; set; }
        public string EMailToDecrypt { get; set; }

        public int? NewsId { get; set; }
        public string NewsTitle { get; set; }

        public string CommentToDisplay
        {
            get
            {
                string Result = "";

                if (!String.IsNullOrWhiteSpace(this.Comment))
                {
                    Result = Comment.Trim();
                }
                else if (!String.IsNullOrWhiteSpace(NewsTitle))
                {
                    Result = "[[[News : ]]]" + NewsTitle;
                }

                return Result;
            }
        }


        public EMailAuditItem()
        {

        }


    }
}
