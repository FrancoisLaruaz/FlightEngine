using Commons;
using Models.Class;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Service.UserArea.Interface;
using DataEntities.Repositories;
using DataEntities.Model;
using Models.ViewModels.Search;
using Models.Class.Search;
using CommonsConst;
using System.Web;
using Commons.Encrypt;

namespace Service.UserArea
{
    public class SearchService : ISearchService
    {

        private readonly IGenericRepository<DataEntities.Model.SearchResult> _searchResultRepo;
        private readonly IGenericRepository<DataEntities.Model.User> _userRepo;
        private readonly Service.Admin.UserRolesService _userRolesService;

        public SearchService(
                IGenericRepository<DataEntities.Model.SearchResult> searchResultRepo,
                IGenericRepository<DataEntities.Model.User> userRepo,
                Service.Admin.UserRolesService userRolesService)
        {
            _searchResultRepo = searchResultRepo;
            _userRepo = userRepo;
            _userRolesService = userRolesService;
        }


        public List<SearchItem> GetSearch(SearchFilter filter, int UserId)
        {
            List<SearchItem> result = new List<SearchItem>();
            try
            {

                int maxCount = 10;

                string term = filter.Pattern ?? "";
                term = term.Replace(".", "");

                bool isAdmin = false;

                User loggedUser = null;
                if (UserId > 0)
                {
                    loggedUser = _userRepo.Get(UserId);
                    if (loggedUser != null)
                    {
                        isAdmin = _userRolesService.IsInRole(UserId, UserRoles.Admin);
                    }
                }

                if (filter.ShowUsers && result.Count < maxCount)
                {
                    List<SearchItem> resultUser = GetSearchUsers(term, null, (maxCount - result.Count));
                    result.AddRange(resultUser);
                }

                if (result.Count < maxCount && filter.ShowPages)
                {
                    List<SearchItem> resultPages = GetSearchPages(term, (maxCount - result.Count), loggedUser, isAdmin);
                    result.AddRange(resultPages);
                }
                int Id = 0;
                int searchId = -1;
                if (!isAdmin)
                    searchId = InsertSearchResultItem(term, UserId, result.Count > maxCount ? maxCount : result.Count, true);

                foreach (var element in result)
                {
                    Id = Id + 1;
                    element.Id = Id;
                    element.SearchId = searchId;
                    element.ImageSrc = element.ImageSrc.Replace("~", "");
                    if (element.Name.Length > 35)
                    {
                        element.Name = element.Name.Substring(0, 30) + "[...]";
                    }
                }

                result = result.OrderBy(i => i.Name).OrderBy(i => i.Order).OrderBy(i => i.TypeOrder).Take(maxCount).ToList();
                if (result.Count >= (maxCount))
                {
                    result.Add(new SearchItem("[[[Show all the results]]]", "/Search", 999999999, SearchItemType.SearchAll));
                    result = result.OrderBy(i => i.Name).OrderBy(i => i.Order).OrderBy(i => i.TypeOrder).ToList();
                }


            }
            catch (Exception e)
            {
                Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, "term = " + filter.Pattern);
            }
            return result;
        }


        public int InsertSearchResultItem(string pattern, int UserId, int SearchResultsCount, bool IsSearchBar)
        {
            int result = -1;
            try
            {


                DateTime now = DateTime.UtcNow;
                DateTime dateToCompare = DateTime.UtcNow.AddMilliseconds(-2500);
                SearchResult search = new SearchResult();

                var searchsToDelete = _searchResultRepo.FindAllBy(u => (UserId <= 0 || u.UserId == UserId) && IsSearchBar == u.IsSearchBar && pattern.ToLower().Trim().Contains(u.Term.ToLower().Trim()) && u.CreationDate > dateToCompare).ToList();
                if (searchsToDelete != null)
                {
                    foreach (var searchToDelete in searchsToDelete)
                    {
                        _searchResultRepo.Delete(searchToDelete);
                    }
                }

                search.Term = pattern ?? "";
                if (UserId > 0)
                    search.UserId = UserId;
                search.CreationDate = now;
                search.IsSearchBar = IsSearchBar;
                search.SearchResultsCount = SearchResultsCount;
                _searchResultRepo.Add(search);
                if (_searchResultRepo.Save())
                {
                    return search.Id;
                }

            }
            catch (Exception e)
            {
                Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, "pattern =" + pattern + " and UserId = " + UserId);
            }
            return result;
        }

        public bool ShowSearchElement(List<string> KeyWords, string term)
        {
            bool result = false;
            try
            {

                term = term.ToLower().Trim().Replace(" ", "").Replace(":", "").Replace(".", "").Replace(";", "").Replace("'", "").Replace("?", "");
                if (KeyWords != null)
                {
                    foreach (var word in KeyWords)
                    {
                        string wordModified = word.ToLower().Trim().Replace(" ", "").Replace(":", "").Replace(".", "").Replace(";", "").Replace("'", "").Replace("?", "");
                        if (wordModified.Contains(term) || term.Contains(wordModified))
                        {
                            return true;
                        }
                    }
                }

            }
            catch (Exception e)
            {
                result = false;
                Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, "term =" + term);
            }
            return result;
        }

        public List<SearchItem> GetSearchPages(string term, int maxCount, User user, bool IsAdmin)
        {
            List<SearchItem> result = new List<SearchItem>();
            try
            {

                if (ShowSearchElement(new List<string> { "password", "reset", "motdepasse", "reinitialiser" }, term))
                    result.Add(new SearchItem("[[[Reset Password]]]", "/MyProfile/password"));
                if (ShowSearchElement(new List<string> { "profile", "preferences", "preference", "profil" }, term))
                    result.Add(new SearchItem("[[[Settings]]]", "/MyProfile/settings"));
                if (ShowSearchElement(new List<string> { "profile", "preferences", "preference", "profil", "email", "username" }, term))
                    result.Add(new SearchItem("[[[Email Preferences]]]", "/MyProfile/emailpreferences"));
                if (ShowSearchElement(new List<string> { "email", "profile", "profil" }, term))
                    result.Add(new SearchItem("[[[Email]]]", "/MyProfile/email"));
                if (ShowSearchElement(new List<string> { "faq", "questions", "frequentlyaskedquestions", }, term))
                    result.Add(new SearchItem("[[[FAQ]]]", "/FAQ"));
                if (ShowSearchElement(new List<string> { "search", "cherche" }, term))
                    result.Add(new SearchItem("[[[Search]]]", "/Search"));
                if (ShowSearchElement(new List<string> { "contactus", "email", "reachout", "help", "aide", "iwanttospeakwithsomeone", "jeveuxparler" }, term))
                    result.Add(new SearchItem("[[[Contact]]]", "/Contact"));
                if (ShowSearchElement(new List<string> { "privacypolicy", "vie privee", "legal", "compliance", "datacollection", "donnees" }, term))
                    result.Add(new SearchItem("[[[Privacy]]]", "/Privacy"));
                if (ShowSearchElement(new List<string> { "termsandconditions", "legal", "compliance", "obligation" }, term))
                    result.Add(new SearchItem("[[[Terms & Conditions]]]", "/Terms"));
            }
            catch (Exception e)
            {
                Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, "term = " + term);
            }
            return result;
        }


        public List<SearchItem> GetSearchUsers(string term, User user, int maxCount)
        {
            List<SearchItem> result = new List<SearchItem>();
            try
            {
                bool byPass = false;
                term = term.ToLower().Trim();
                List<User> list = new List<User>();

                list = _userRepo.FindAllBy(u => u.PublicProfile).OrderByDescending(u => u.DateLastConnection).ToList();


                foreach (var item in list)
                {
                    if (maxCount > 0)
                    {
                        string firstNameDecrypted = item.FirstName;
                        string lastNameDecrypted =item.LastName;
                        string fullNameDecrypted = firstNameDecrypted + " " + lastNameDecrypted;
                        string fullNameDecryptedToLower = firstNameDecrypted.ToLower().Trim() + " " + lastNameDecrypted.ToLower().Trim();
                        string fullNameDecryptedReverseToLower = lastNameDecrypted.ToLower().Trim() + " " + firstNameDecrypted.ToLower().Trim();
                        if (byPass || (firstNameDecrypted.ToLower().Trim().Contains(term) || lastNameDecrypted.ToLower().Trim().Contains(term) || fullNameDecryptedReverseToLower.Contains(term) || fullNameDecryptedToLower.Contains(term) ))
                        {

                            SearchItem searchItem = new SearchItem();
                            searchItem.SearchItemType = SearchItemType.User;
                            searchItem.TypeOrder = 2;
                            searchItem.ItemId = item.Id;
                            searchItem.Name = fullNameDecrypted;
                            searchItem.LoggedUserId = user == null ? 0 : user.Id;
                            searchItem.ImageSrc = item.PictureSrc ?? CommonsConst.DefaultImage.DefaultThumbnailUser;
                            searchItem.ItemFollowed = user == null ? false : user.UserFollows1.Where(f => f.FollowedUserId == item.Id).Any();
                            if (!System.IO.File.Exists(HttpContext.Current.Server.MapPath(searchItem.ImageSrc)))
                            {
                                searchItem.ImageSrc = CommonsConst.DefaultImage.DefaultThumbnailUser.Replace("~","");
                            }
                            searchItem.ImageSrc = searchItem.ImageSrc.Replace("~", "");
                            searchItem.Description = item.Description;
                            searchItem.Url = "/MyProfile/" + item.Id;
                            searchItem.ShortDescription = "";
                            string city = "";
                            string province = item.Address?.Province?.Name;


                            if (!String.IsNullOrWhiteSpace(city))
                            {
                                searchItem.ShortDescription = searchItem.ShortDescription + city;
                            }
                            if (!String.IsNullOrWhiteSpace(province))
                            {
                                if (!String.IsNullOrWhiteSpace(searchItem.ShortDescription))
                                {
                                    searchItem.ShortDescription = searchItem.ShortDescription + ", ";
                                }
                                searchItem.ShortDescription = searchItem.ShortDescription + province;
                            }

                            result.Add(searchItem);
                            maxCount--;
                        }
                    }
                    else
                    {
                        break;
                    }
                }

            }
            catch (Exception e)
            {
                Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, "term = " +term);
            }
            return result;
        }


        public SearchIndexResultViewModel GetSearchIndexResultViewModel(SearchFilter filter, int UserId)
        {
            SearchIndexResultViewModel model = new SearchIndexResultViewModel();
            try
            {
                int maxCount = 999999;

                string term = filter.Pattern ?? "";


                bool isAdmin = false;

                User loggedUser = null;
                if (UserId > 0)
                {
                    loggedUser = _userRepo.Get(UserId);
                    if (loggedUser != null)
                    {
                        isAdmin = _userRolesService.IsInRole(UserId, UserRoles.Admin);
                    }
                }
                int searchId = -1;
                if (!isAdmin)
                    searchId = InsertSearchResultItem(term, UserId, 0, false);


                if (filter.ShowUsers)
                {
                    model.Users = new SearchIndexResultTypeViewModel(fixSearchItemsList(GetSearchUsers(term, loggedUser, maxCount), maxCount, searchId), "[[[Users]]]", "user");
                }
                if (filter.ShowPages)
                {
                    model.Pages = new SearchIndexResultTypeViewModel(fixSearchItemsList(GetSearchPages(term, maxCount, loggedUser, isAdmin), maxCount, searchId), "[[[Pages]]]", "globe");
                }
                model.SearchResultsGeneralCount = model.Pages.Items.Count + model.Users.Items.Count;

                SearchResult searchItem = _searchResultRepo.Get(searchId);
                if (searchItem != null)
                {
                    searchItem.SearchResultsCount = model.SearchResultsGeneralCount;
                    _searchResultRepo.Edit(searchItem);
                    _searchResultRepo.Save();
                }


            }
            catch (Exception e)
            {
                Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, "term = " + filter.Pattern);
            }
            return model;
        }


        public List<SearchItem> fixSearchItemsList(List<SearchItem> list, int maxCount, int searchId)
        {
            try
            {
                list = list.OrderBy(i => i.Name).OrderBy(i => i.Order).Take(maxCount).ToList();
                int Id = 0;
                foreach (var element in list)
                {
                    Id = Id + 1;
                    element.Id = Id;
                    element.SearchId = searchId;
                }
            }
            catch (Exception e)
            {
                Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, "searchId = " + searchId);
            }
            return list;
        }

        public bool SetUrlClickedForSearch(int SearchId, string Url)
        {
            bool result = true;
            try
            {
                if (SearchId > 0 && !String.IsNullOrWhiteSpace(Url))
                {
                    SearchResult search = _searchResultRepo.Get(SearchId);
                    if (search != null && String.IsNullOrWhiteSpace(search.ClickedUrl))
                    {
                        search.ClickedUrl = Url;
                        _searchResultRepo.Edit(search);
                        result = _searchResultRepo.Save();
                    }
                }
            }
            catch (Exception e)
            {
                Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, "SearchId = " + SearchId + " and Url = " + Url);
            }
            return result;
        }


    }
}
