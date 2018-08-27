using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Models.Class.Search;

namespace Models.ViewModels.Profile
{
    public class GeneralInformationViewModel
    {
        public GeneralInformationViewModel()
        {

        }



        public bool CanUserEditProfile { get; set; }
        public int UserId { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string CreationDateTxt { get; set; }

        public string Facebook { get; set; }

        public string Description { get; set; }

        public string City { get; set; }

        public string Province { get; set; }

        public string Country { get; set; }

        public string BackgroundPictureSrc { get; set; }

        public string ImageSrc { get; set; }

        public bool IsLoggedUserProfile { get; set; }

        public bool FollowedUser { get; set; }

        public DateTime LastConnectionDate { get; set; }

        public string Location
        {

            get
            {
                string result = "";
                if (!String.IsNullOrWhiteSpace(this.City))
                {
                    result = result + this.City;
                }
                if (!String.IsNullOrWhiteSpace(this.Province))
                {
                    if (!String.IsNullOrWhiteSpace(result))
                    {
                        result = result + ", ";
                    }
                    result = result + this.Province;
                }


                return result;
            }

        }

    }
}
