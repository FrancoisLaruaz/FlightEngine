using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Models.Class.ExternalAuthentification
{
    public class ExternalSignUpInformation
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }

        public string FacebookLink { get; set; }

        public string ImageSrc { get; set; }


        public string FacebookId { get; set; }

        public string CountryCode { get; set; }

        public bool EmailPermission { get; set; }

        public string LoginProvider { get; set; }

        public string ProviderKey { get; set; }

        public List<string> FriendsList { get; set; }

        public int? GenderId { get; set; }



        public bool FriendsPermission { get; set; }

        public ExternalSignUpInformation()
        {
            EmailPermission = true;
            FriendsPermission = true;
            FriendsList = new List<string>();
        }
    }
}
