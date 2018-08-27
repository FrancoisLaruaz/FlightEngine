using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;


namespace Models.ViewModels.Account
{
    public class SignUpViewModel
    {
        [Required(ErrorMessage = "[[[The Last Name is required.]]]")]
        [Display(Name = "[[[Last Name]]]")]
        public string LastName { get; set; }


        [Required(ErrorMessage = "[[[The First Name is required.]]]")]
        [Display(Name = "[[[First Name]]]")]
        public string FirstName { get; set; }


        [Required(ErrorMessage = "[[[A valid email is required.]]]")]
        [DataType(DataType.EmailAddress)]
        [Display(Name = "[[[Email]]]")]
        public string Email { get; set; }

        [Required(ErrorMessage = "[[[The Password field is required.]]]")]
        [DataType(DataType.Password)]
        [Display(Name = "[[[Password]]]")]
        public string Password { get; set; }

        public string LangTagPreference { get; set; }

        public bool ReceiveNews { get; set; }


        public SignUpViewModel()
        {
        }
    }
}
