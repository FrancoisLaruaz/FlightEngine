using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Models.Class;

namespace Models.ViewModels.Admin.Tasks
{
    public class DisplayTasksViewModel : BaseModelPager
    {
        public List<DataEntities.Model.TaskLog> TaskList { get; set; }

        public string Pattern { get; set; }


        public DisplayTasksViewModel()
        {
            TaskList = new List<DataEntities.Model.TaskLog>();
        }
    }
}
