using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using Models.Class;

namespace Models.Class.UserFollow
{
    public class UserFollowItem
    {
        public UserFollowItem()
        {

        }

        public int UserId { get; set; }


        public int Id { get; set; }


        public string FirstNameUser { get; set; }

        public string LastNameUser { get; set; }

        public string PictureSrcUser { get; set; }

        public string Url { get; set; }

    }

}
