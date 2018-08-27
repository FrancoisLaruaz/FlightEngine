using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Models.Class;
using Models.Class.Search;

namespace Models.ViewModels.Search
{
    public class SearchIndexViewModel
    {
        public SearchIndexViewModel()
        {

        }

        public bool ShowUsers { get; set; }


        public bool ShowPages { get; set; }


        public string Pattern { get; set; }


    }
}
