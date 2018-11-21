using Commons;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models.ViewModels;
using DataEntities.Repositories;
using DataEntities.Model;
using Models.ViewModels.Account;
using Models.ViewModels.Home;
using Models.Class.Search;
using Models.ViewModels.Search;

namespace Service.UserArea.Interface
{
    public interface ISearchService
    {
        List<SearchItem> GetSearch(SearchFilter filter, int UserId, string BrowserLanguage);

        bool SetUrlClickedForSearch(int SearchId, string Url);

        SearchIndexResultViewModel GetSearchIndexResultViewModel(SearchFilter filter, int UserId, string BrowserLanguage);

    }
}
