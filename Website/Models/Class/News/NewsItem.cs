using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Models.Class.News
{
    public class NewsItem
    {
        public int Id { get; set; }
        public DateTime PublishDate { get; set; }

        public DateTime CreationDate { get; set; }

        public DateTime ModificationDate { get; set; }

        public int? LastModificationUserId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }

        public string MailSubject { get; set; }

        public int TypeId { get; set; }

        public bool Active { get; set; }

        public int? TypeUserMailingId { get; set; }

        public string TypeName { get; set; }

        public string TypeUserMailingName { get; set; }

        public int? ScheduledTaskId { get; set; }

        public bool? HasScheduledTaskBeenExecuted { get; set; }

        public int MailSentNumber { get; set; }


        public string LastModificationUserFullNameDecrypt { get; set; }


        public NewsItem()
        {

        }


    }
}
