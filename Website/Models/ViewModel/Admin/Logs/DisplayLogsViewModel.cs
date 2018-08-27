using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Models.Class;
using  DataEntities.Model;

namespace Models.ViewModels.Admin.Logs
{
    public class DisplayLogsViewModel : BaseModelPager
    {
        public List<Log4Net> LogsList { get; set; }

        public string Pattern { get; set; }


        public DisplayLogsViewModel()
        {
            LogsList = new List<Log4Net>();
        }
    }
}
