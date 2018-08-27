using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Models.Class
{
    [Serializable]
    public class UserSession
    {
        public string UserName { get; set; }


        public string UserIdentityId { get; set; }

        public int UserId { get; set; }

        public string FirstName { get; set; }

       public string LastName { get; set; }

        public string LanguageTag { get; set; }

        public string PictureThumbnailSrc { get; set; }

        public DateTime DateLastConnection { get; set; }

        public bool EmailConfirmed { get; set; }

        public string UserFullName { get { return (FirstName + " " + LastName).Trim(); } }

        public UserSession()
        {

        }
    }
}
