using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Models.Class;
using Models.Class.Search;
using Models.Class.Localization;

namespace Models.ViewModels.Browse
{
    public class BrowseIndexViewModel
    {
        public BrowseIndexViewModel()
        {

        }

        public string Language { get; set; }

        public string LocalizationJson { get; set; }

        public string ProductsListJson { get; set; }

    }
}
