using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Models.Class.Search;
using Models.Class.UserFollow;

namespace Models.ViewModels.Profile
{
    public class FollowersViewModel
    {
        public FollowersViewModel()
        {
            Followers = new List<UserFollowItem>();
        }


        public FollowersViewModel(string _Title, List<UserFollowItem> _Followers)
        {
            Title = _Title;
            Followers = _Followers;
        }

        public bool IsLoggedUserProfile { get; set; }

        public string Title { get; set; }

        public List<UserFollowItem> Followers { get; set; }

    }
}
