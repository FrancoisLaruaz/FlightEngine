using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using Models.Class;

namespace Models.Class.UserRoles
{
    public  class UserRoleItem
    {
        public string UserLastNameDecrypt { get; set; }
        public string UserFirstNameDecrypt { get; set; }
        public string UserNameDecrypt { get; set; }
        public int UserId { get; set; }
        public string UseridentityId { get; set; }
        public DateTime DateLastConnection { get; set; }

        public List<RoleItem> UserRolesList { get; set; }

        public List<RoleItem> UserNotInRoleList { get; set; }

        public UserRoleItem()
        {
            UserRolesList = new List<RoleItem>();
            UserNotInRoleList = new List<RoleItem>();
        }
    }
}
