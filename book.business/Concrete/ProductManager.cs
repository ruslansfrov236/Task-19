using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using book.business.Abstract;
using book.data.Abstract;

using book.entity;

namespace book.business.Concrete
{
    public class ProductManager : IProductService
    {

        // EfCoreProductRepository productRepository= new EfCoreProductRepository();
        private IProductRepository _productRepository;

        public ProductManager(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public string ErrorMessage { get; set; }
        public bool Create(Product entity)
        {
            if (Validation(entity))
            {
                _productRepository.Create(entity);
                return true;
            }
            return false;

        }

        public void Delete(Product entity)
        {
            _productRepository.Delete(entity);
        }



        public List<Product> GetAll()
        {
            return _productRepository.GetAll();
        }

        public Product GetById(int id)
        {
            return _productRepository.GetById(id);
        }

        public Product GetByIdWithCategories(int id)
        {
            return _productRepository.GetByIdWithCategories(id);
        }

        public int GetCountByCategory(string category)
        {
            return _productRepository.GetCountByCategory(category);
        }

        public List<Product> GetHomePageProducts()
        {
            return _productRepository.GetHomePageProducts();
        }

        public Product GetProductDetails(string url)
        {
            return _productRepository.GetProductDetails(url);
        }

        public List<Product> GetProductsByCategory(string name, int page, int pageSize)
        {
            return _productRepository.GetProductsByCategory(name, page, pageSize);
        }

        public List<Product> GetSearchResult(string searchString)
        {
            return _productRepository.GetSearchResult(searchString);

        }

        

        public bool Update(Product entity, int[] categoryId)
        {
            if (Validation(entity))
            {
                if(categoryId.Length==0){
                    ErrorMessage +="Mehsul ucun bir kategoriya secim etmek lazimdir .\n";
                    return false;
                }
               _productRepository.Update(entity, categoryId);
                return true;
            }
            return false;
            
        }

        public void Update(Product entity)
        {
            _productRepository.Update(entity);
        }

        public bool Validation(Product entity)
        {
            var IsValid = true;
            if (string.IsNullOrEmpty(entity.Name))
            {
                ErrorMessage += "mehsul adini daxil etmek teleb edilir .\n";
                IsValid = false;
            }

            if (entity.Price < 0 && entity.Price == null)
            {
                ErrorMessage += "menfi olmayan eded daxil olmasi teleb edilir .\n";

                ErrorMessage += "qiymet daxil etmek teleb edilir .\n";
                IsValid = false;
            }
            return IsValid;
        }

        

      
    }
}
