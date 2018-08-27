using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using Models.Class;

namespace Models.Class.Search
{
    public class SearchItem
    {
        public SearchItem()
        {
            this.Order = 99;
            this.TypeOrder = 99;
        }

        public SearchItem(string _Name, string _Url)
        {
            this.TypeOrder = 5;
            this.Name = _Name;
            this.Url = _Url;
            this.ImageSrc = CommonsConst.DefaultImage.Page;
            this.SearchItemType = CommonsConst.SearchItemType.Page;
        }



        public SearchItem(string _Name, string _Url, int _TypeOrder, string _SearchItemType)
        {
            this.TypeOrder = _TypeOrder;
            this.Name = _Name;
            this.Url = _Url;
            this.SearchItemType = _SearchItemType;
        }

        public int TypeOrder { get; set; }
        public int Order { get; set; }
        public string Url { get; set; }

        public string ImageSrc { get; set; }
        public string Name { get; set; }
        public string SearchItemType { get; set; }

        public string Description { get; set; }

        public string ShortDescription { get; set; }

        public int Id { get; set; }

        public bool IsPrivate { get; set; }

        public bool ItemFollowed { get; set; }

        public int ItemId { get; set; }

        public int SearchId { get; set; }

        public int LoggedUserId { get; set; }
    }
}
