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

namespace Service.UserArea
{
    public  class CategoryService : ICategoryService
    {


        private readonly IGenericRepository<Category> _categoryRepo;

        public CategoryService(IGenericRepository<Category> categoryRepo)
        {
            _categoryRepo = categoryRepo;
        }

        public CategoryService()
        {
            var context = new TemplateEntities();
            _categoryRepo = new GenericRepository<DataEntities.Model.Category>(context);
        }

        /// <summary>
        /// Get a list for a dropdownlist for the requested category
        /// </summary>
        /// <param name="categoryTypeId"></param>
        /// <returns></returns>
        public  List<SelectionListItem> GetSelectionList(int CategoryTypeId)
        {
            List<SelectionListItem> newList = new List<SelectionListItem>();
            try
            {
                List<Category> categories = GeListCategoriesByType(CategoryTypeId);
                foreach (var item in categories)
                {
                    newList.Add(new SelectionListItem(item.Id, item.Name, item.Code));
                }
            }
            catch (Exception e)
            {
                Commons.Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, "CategoryTypeId = " + CategoryTypeId);
            }
            return newList;
        }

        public  List<Category> GeListCategories()
        {
            List<Category> result = null;
            try
            {
                result = _categoryRepo.List().ToList();
            }
            catch (Exception e)
            {
                result = new List<Category>(); 
                Commons.Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
            }
            return result;
        }

        public  Category GetCategoryById(int Id)
        {
            Category result = null;
            try
            {
                result = _categoryRepo.Get(Id);
            }
            catch (Exception e)
            {
                result = null;
                Commons.Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, "Id = " + Id.ToString());
            }
            return result;
        }

        public  Category GetCategoryByCode(string Code)
        {
            Category result = null;
            try
            {
                result = _categoryRepo.FindAllBy(c => c.Code.Trim().ToLower() == Code.Trim().ToLower()).FirstOrDefault();
            }
            catch (Exception e)
            {
                result = null;
                Commons.Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, "Code = " + Code);
            }
            return result;
        }

        public  List<Category> GeListCategoriesByType(int CategoryTypeId,bool ActiveOnly=true)
        {
            List<Category> result = null;
            try
            {
                result = _categoryRepo.FindAllBy(c => c.CategoryTypeId== CategoryTypeId && (c.Active || ActiveOnly==false)).ToList();
            }
            catch (Exception e)
            {
                result = null;
                Commons.Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
            }
            return result;
        }

        /// <summary>
        /// Delete the specified category
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public  bool DeleteCategoryById(int Id)
        {
            bool result = false;
            try
            {

                Category category = _categoryRepo.Get(Id);
                if(category!=null)
                {
                    _categoryRepo.Delete(category);
                    result = _categoryRepo.Save(); 
                }

      
            }
            catch (Exception e)
            {
                result = false;
                Commons.Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, "Id = " + Id.ToString());
            }
            return result;
        }

    }
}
