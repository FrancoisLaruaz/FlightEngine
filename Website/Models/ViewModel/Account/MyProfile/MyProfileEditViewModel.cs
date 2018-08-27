using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

using Models.Class;

namespace Models.ViewModels.Account
{
    public class MyProfileEditViewModel
    {
        public int UserId { get; set; }

        [Required(ErrorMessage = "[[[The first name is required.]]]")]
        [Display(Name = "[[[First name]]]")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "[[[The last name is required.]]]")]
        [Display(Name = "[[[Last name]]]")]
        public string LastName { get; set; }

        [Display(Name = "[[[Description]]]")]
        public string Description { get; set; }

        [Display(Name = "[[[Receive News Letter]]]")]
        public bool ReceiveNews { get; set; }

        [Display(Name = "[[[User Name]]]")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "[[[The prefefered language is required.]]]")]
        [Display(Name = "[[[Language]]]")]
        public int LanguageId { get; set; }

        [Display(Name = "[[[Gender]]]")]
        public int? GenderId { get; set; }

        [Display(Name = "[[[Date Of Birth]]]")]
        public DateTime? DateOfBirth { get; set; }


        public List<SelectionListItem> GenderList { get; set; }
        public List<SelectionListItem> LanguageList { get; set; }

        public MyProfileEditViewModel()
        {
            GenderList = new List<SelectionListItem>();
            LanguageList = new List<SelectionListItem>();
        }
    }
}
