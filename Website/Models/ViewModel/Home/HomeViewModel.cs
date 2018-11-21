using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Models.ViewModels.Home
{
    public class HomeViewModel
    {
        public bool SignUp { get; set; }

        public bool PromptLogin { get; set; }

        public string RedirectTo { get; set; }

        public string SliderHomePageJson { get; set; }

        public bool LogOff { get; set; }

        public HomeViewModel()
        {
           
        }
    }
}
