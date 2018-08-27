using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Models.Class.TaskSchedule;


namespace Models.ViewModels.Admin.Tasks
{
    public class SchedulerStatusViewModel 
    {


        [Display(Name = "Is Scheduler Active")]
        public bool IsSchedulerActive { get; set; }

        [Display(Name = "Scheduled Tasks Number In Scheduler")]
        public int ScheduledTasksNumberInScheduler { get; set; }


        [Display(Name = "Scheduled Tasks Number In Database")]
        public int ScheduledTasksNumberInDatabase { get; set; }


        [Display(Name = "Not executed tasks")]
        public int ScheduledTasksProblemsNumber { get; set; }

        public List<RecurringTask> RecurringTaskList { get; set; }

        public SchedulerStatusViewModel()
        {
            IsSchedulerActive = true;
            RecurringTaskList = new List<RecurringTask>();
            ScheduledTasksNumberInDatabase = 0;
            ScheduledTasksNumberInScheduler = 0;
            ScheduledTasksProblemsNumber = 0;
        }
    }
}
