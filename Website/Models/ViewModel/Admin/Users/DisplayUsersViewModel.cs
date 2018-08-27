using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Models.Class;
using Models.Class.UserRoles;

namespace Models.ViewModels.Admin.Users
{
    public class DisplayUsersViewModel : BaseModelPager
    {
        public List<UserRoleItem> UserRolesList { get; set; }

        public string Pattern { get; set; }


        public DisplayUsersViewModel()
        {
            UserRolesList = new List<UserRoleItem>();
        }
    }
}
