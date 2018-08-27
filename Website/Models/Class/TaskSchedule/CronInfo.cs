using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Models.Class.TaskSchedule
{
    public class CronInfo
    { 

        public string Periodicity { get; set; }

        public string ScheduleInfo { get; set; }


        public CronInfo()
        {
           
        }


    }
}
