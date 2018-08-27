using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;


namespace Models.ViewModels.Home
{
    public class ContactUsViewModel
    {
        [Display(Name = "[[[Phone number]]]")]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "[[[The question is required.]]]")]
        [Display(Name = "[[[Question]]]")]
        public string Question { get; set; }

        [Required(ErrorMessage = "[[[The subject is required.]]]")]
        [Display(Name = "[[[Subject]]]")]
        public string Subject { get; set; }


        [Required(ErrorMessage = "[[[The email is required.]]]")]
        [Display(Name = "[[[Email]]]")]
        public string Email { get; set; }

        [Required(ErrorMessage = "[[[The name is required.]]]")]
        [Display(Name = "[[[Name]]]")]
        public string Name { get; set; }

        public ContactUsViewModel()
        {
           
        }
    }
}
