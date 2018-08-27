using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Models.Class.Search;

namespace Models.ViewModels.Profile
{
    public class PublicProfileViewModel
    {
        public PublicProfileViewModel()
        {
            CanUserEdit = false;
        }

        public string Show { get; set; }
        public int UserId { get; set; }

        public bool CanUserEdit { get; set; }

        public bool IsLoggedUserProfile { get; set; }

        public string UpdateFormAuthorityJson { get; set; }

    }
}
