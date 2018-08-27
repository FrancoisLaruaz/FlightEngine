using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;


namespace Models.ViewModels.Account
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "[[[A valid email is required.]]]")]
        [DataType(DataType.EmailAddress)]
        [Display(Name = "[[[Email]]]")]
        public string Email { get; set; }

        [Required(ErrorMessage = "[[[The Password field is required.]]]")]
        [DataType(DataType.Password)]
        [Display(Name = "[[[Password]]]")]
        public string Password { get; set; }

        [Display(Name = "[[[Remember me ?]]]")]
        public bool RememberMe { get; set; }

        public string URLRedirect { get; set; }

        public string LanguageTag { get; set; }

        public bool IsUserAlreadyLoggedIn { get; set; }

        public LoginViewModel()
        {
            RememberMe = true;
        }
    }
}
