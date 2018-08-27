using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

using Models.Class;

namespace Models.ViewModels.Account
{
    public class MyProfileAddressViewModel
    {
        public int UserId { get; set; }


        [Display(Name = "[[[Address]]]")]
        public string Adress1 { get; set; }

        [Display(Name = "[[[Address]]]")]
        public string Adress2 { get; set; }

        [Display(Name = "[[[Address]]]")]
        public string Adress3 { get; set; }

        [Display(Name = "[[[Country]]]")]
        public int? CountryId { get; set; }

        [Display(Name = "[[[Province]]]")]
        public int? ProvinceId { get; set; }

        public List<SelectionListItem> CountryList { get; set; }
        public List<SelectionListItem> ProvinceList { get; set; }

        public MyProfileAddressViewModel()
        {
            CountryList= new List<SelectionListItem>();
            ProvinceList = new List<SelectionListItem>();
        }
    }
}
