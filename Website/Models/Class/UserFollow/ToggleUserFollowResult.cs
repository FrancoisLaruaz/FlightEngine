using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using Models.Class;

namespace Models.Class.UserFollow
{
    public class ToggleUserFollowResult
    {
        public ToggleUserFollowResult()
        {

        }

        public bool Result { get; set; }

        public bool Followed { get; set; }


    }

}
