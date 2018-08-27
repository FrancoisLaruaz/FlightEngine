using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Models.ViewModels.Search
{
    public class SearchIndexResultViewModel
    {
        public SearchIndexResultViewModel()
        {
            Users = new SearchIndexResultTypeViewModel();
            Pages = new SearchIndexResultTypeViewModel();
        }

        public int SearchResultsGeneralCount { get; set; }

        public SearchIndexResultTypeViewModel Users { get; set; }

        public SearchIndexResultTypeViewModel Pages { get; set; }




    }
}
