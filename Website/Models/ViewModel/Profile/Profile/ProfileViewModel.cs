using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Models.Class.Search;

namespace Models.ViewModels.Profile
{
    public class ProfileViewModel
    {
        public ProfileViewModel()
        {
            GeneralInformation = new GeneralInformationViewModel();
            PeopleYouFollow = new FollowersViewModel();
            Followers = new FollowersViewModel();
        }


        public FollowersViewModel PeopleYouFollow { get; set; }

        public FollowersViewModel Followers { get; set; }

        public bool CanUserEditProfile { get; set; }
        public int UserId { get; set; }


        public bool IsUserFollowed { get; set; }

        public GeneralInformationViewModel GeneralInformation { get; set; }
    }
}
