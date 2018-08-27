using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Models.Class;
using Models.ViewModels;
using DataEntities.Model;
using Models.Class.News;

namespace Models.ViewModels.Admin.News
{
    public class DisplayPublishedNewsViewModel : BaseModelPager
    {
        public List<NewsItem> NewsList { get; set; }

        public string Pattern { get; set; }


        public DisplayPublishedNewsViewModel()
        {
            NewsList = new List<NewsItem>();
        }
    }
}
