using Commons;
using Models.Class;
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

namespace Service.UserArea.Interface
{
    public interface ICategoryService
    {
        List<SelectionListItem> GetSelectionList(int CategoryTypeId);

        List<Category> GeListCategories();

        Category GetCategoryByCode(string Code);

        bool DeleteCategoryById(int Id);

        List<Category> GeListCategoriesByType(int CategoryTypeId, bool ActiveOnly = true);

    }
}
