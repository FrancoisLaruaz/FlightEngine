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
using Models.Class.Product;
using System.Configuration;
using CommonsConst;

namespace Service.UserArea
{
    public  class ProductService : IProductService
    {

        private readonly IGenericRepository<User> _userRepo;
        private readonly IGenericRepository<Product> _productRepo;

        private static string WebsiteURL = ConfigurationManager.AppSettings["Website"];

        public ProductService(IGenericRepository<User> userRepo, IGenericRepository<Product> productRepo)
        {
            _userRepo = userRepo;
            _productRepo = productRepo;
        }


        public List<ProductItem> GetProducts(ProductFilter filter)
        {
            List<ProductItem> result = new List<ProductItem>();
            try
            {
                var productsList = _productRepo.FindAllBy(p => p.StatusId == ProductStatus.Active).ToList();
                foreach(var product in productsList)
                {
                    bool addToList = true;
                    if(filter.ProductSubtypeId!=null && product.SubTypeId!=filter.ProductSubtypeId.Value)
                    {
                        addToList = false;
                    }
                    else  if(filter.ProductTypeId != null && product.ProductSubType?.ProductType.Id != filter.ProductTypeId.Value)
                    {
                        addToList = false;
                    }
                    if(addToList)
                        result.Add(TransformProductIntoProductItem(product));
                }
            }
            catch (Exception e)
            {
                Commons.Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, "longitude = " +  filter.Longitude + " and  latitude = " +  filter.Latitude+" and distancemax = "+filter.DistanceMax);
            }
            return result;
        }


        public ProductItem TransformProductIntoProductItem(Product product)
        {
            ProductItem result = new ProductItem();
            try
            {
                if (product != null)
                {
                    Address address = product.Address;
                    result.ProductSubtypeId = product.SubTypeId;
                    result.Id = product.Id;
                    result.CreatorUserId = product.UserId;
                    result.CreatorUserName = product.CreatorUser.FirstName + " " + product.CreatorUser.LastName;
                    result.City = address.City;
                    result.Venue = address.Venue;
                    result.Unit = address.Venue;
                    result.PostalCode = result.PostalCode;
                    result.Street = address.Street;
                    result.SubProvinceId = address.SubProvinceId;
                    result.SubProvinceName = address.SubProvince?.Name;
                    result.ProvinceId = address.SubProvinceId.HasValue ? address.SubProvince.ProvinceId : address.ProvinceId;
                    result.ProvinceName = address.SubProvinceId.HasValue ? address.SubProvince.Province.Name : address.Province.Name;
                    result.CountryId = address.SubProvinceId.HasValue ? address.SubProvince.Province.CountryId : address.Province.CountryId;
                    result.CountryName = address.SubProvinceId.HasValue ? address.SubProvince.Province.Country.Name : address.Province.Country.Name;
                    result.ImageSrc = product.ImageSrc;
                    result.Latitude = Convert.ToDouble(address.Latitude);
                    result.Longitude = Convert.ToDouble(address.Longitude);
                    result.ProductSubtypeId = product.SubTypeId;
                    result.ProductSubtypeName = product.ProductSubType.Name;
                    result.ProductTypeId = product.ProductSubType.ProductTypeId;
                    result.ProductTypeName = product.ProductSubType.ProductType.Name;
                    result.Title = product.Title;
                    result.Description = product.Description;
                }
            }
            catch (Exception e)
            {
                result = null;
                Commons.Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, " Id = " + product.Id);
            }
            return result;
        }

 
    }
}
