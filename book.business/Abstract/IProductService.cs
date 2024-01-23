using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using book.entity;

namespace book.business.Abstract
{
    public interface IProductService : IValidator<Product>
    {
        Product GetById(int id);
        Product GetProductDetails(string url);
        List<Product> GetSearchResult(string searchString);
        List<Product> GetProductsByCategory(string name, int page, int pageSize);

        List<Product> GetAll();
        bool Create(Product entity);
        void Update(Product entity);
        bool Update(Product entity, int[] categoryId);
        void Delete(Product entity);
        int GetCountByCategory(string category);
        List<Product> GetHomePageProducts();
        Product GetByIdWithCategories(int id);
    }
}