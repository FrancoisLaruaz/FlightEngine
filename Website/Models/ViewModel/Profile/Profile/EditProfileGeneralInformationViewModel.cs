using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Models.Class.Search;

namespace Models.ViewModels.Profile
{
    public class EditGeneralInformationViewModel
    {
        public EditGeneralInformationViewModel()
        {

        }


        public int UserId { get; set; }

        [Display(Name = "[[[First Name]]]")]
        public string FirstName { get; set; }

        [Display(Name = "[[[Last Name]]]")]
        public string LastName { get; set; }

        [Display(Name = "[[[Facebook]]]")]
        public string Facebook { get; set; }

        [Display(Name = "[[[Bio]]]")]
        public string Description { get; set; }



    }
}
