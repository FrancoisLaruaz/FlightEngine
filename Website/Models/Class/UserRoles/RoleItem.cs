using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using Models.Class;

namespace Models.Class.UserRoles
{
    public  class RoleItem
    {
        public string RoleId { get; set; }
        public string RoleName { get; set; }

        public RoleItem()
        {
            
        }
    }
}
