using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Models.Class.SignUp
{

    public class UserSignUp
    {
        public string UserName { get; set; }

        public string UserIdentityId { get; set; }

        public int UserId { get; set; }

        public string FirstName { get; set; }

       public string LastName { get; set; }

        public string FacebookLink { get; set; }

        public string LanguageTag { get; set; }

        public string PictureSrc { get; set; }

        public string EmailConfirmationToken { get; set; }

        public int? GenderId { get; set; }

        public int? LanguageId { get; set; }

        public bool ReceiveNews { get; set; }


        public UserSignUp()
        {

        }
    }
}
