using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using DataEntities.Model;
using Models.Class.News;

namespace Models.ViewModels.Admin.News
{
    public class NewsViewModel 
    {

        public List<NewsItem> NotPublishedNews { get; set; }


        public NewsViewModel()
        {
            NotPublishedNews = new List<NewsItem>();
        }
    }
}
