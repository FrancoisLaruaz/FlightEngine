using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;


namespace Models.ViewModels.Account
{
    public class ChangePasswordViewModel
    {
        [Required(ErrorMessage = "[[[The old password is required.]]]")]
        [Display(Name = "[[[Old Password]]]")]
        public string OldPassword { get; set; }

        [Required(ErrorMessage = "[[[The new password is required.]]]")]
        [Display(Name = "[[[New Password]]]")]
        public string NewPassword { get; set; }


        public int UserId { get; set; }

        public ChangePasswordViewModel()
        {
            
        }
    }
}
