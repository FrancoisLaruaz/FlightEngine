using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Models.Class;
using Models.Class.Email;

namespace Models.ViewModels.Admin.Email
{
    public class DisplayEmailAuditViewModel : BaseModelPager
    {
        public List<EMailAuditItem> AuditsList { get; set; }

        public string Pattern { get; set; }


        public DisplayEmailAuditViewModel()
        {
            AuditsList = new List<EMailAuditItem>();
        }
    }
}
