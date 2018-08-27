using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Models.Class.TaskSchedule
{
    public class RecurringTask
    {
        public string GroupName { get; set; }

        public string Periodicity { get; set; }

        public string ScheduleInfo { get; set; }


        public RecurringTask()
        {
            
        }

        public RecurringTask(string _GroupName, string _Periodicity, string _ScheduleInfo)
        {
            this.GroupName = _GroupName;
            this.Periodicity = _Periodicity;
            this.ScheduleInfo = _ScheduleInfo;
        }

    }
}
