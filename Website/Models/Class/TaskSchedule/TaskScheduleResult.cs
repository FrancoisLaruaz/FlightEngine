using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Models.Class.TaskSchedule
{
    public class TaskScheduleResult
    { 

        public string GroupName { get; set; }

        public string Id { get; set; }

        public bool Result { get; set; }

        public TaskScheduleResult()
        {
            Result = false;
        }


    }
}
