using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Models.Class.Search;

namespace Models.ViewModels.Search
{
    public class SearchIndexResultTypeViewModel
    {
        public SearchIndexResultTypeViewModel()
        {

            Items = new List<SearchItem>();
        }

        public SearchIndexResultTypeViewModel(List<SearchItem> _items, string _title, string _icon)
        {

            Items = _items;
            Title = _title;
            Icon = _icon;
        }


        public List<SearchItem> Items { get; set; }

        public string Title { get; set; }

        public string Icon { get; set; }


    }
}
