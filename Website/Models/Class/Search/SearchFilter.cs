using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using Models.Class;

namespace Models.Class.Search
{
    public class SearchFilter
    {
        public SearchFilter()
        {

        }

        public SearchFilter(string _pattern)
        {
            this.Pattern = _pattern;
            this.ShowUsers = true; 
            this.ShowPages = true; 
        }


        public bool ShowUsers { get; set; }

        public bool ShowPages { get; set; }
        public string Pattern { get; set; }

    }
}
