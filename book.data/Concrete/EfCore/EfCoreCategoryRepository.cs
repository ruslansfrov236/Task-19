using System.Collections.Generic;
using book.data.Abstract;
using book.entity;
using Microsoft.EntityFrameworkCore;

namespace book.data.Concrete.EfCore
{
    public class EfCoreCategoryRepository : EfCoreGenericRepository<Category, ShopContext>, ICategoryRepository
    {
        public void DeleteFromCategory(int productId, int categoryId)
        {
            using (var context = new ShopContext())
            {
                var cmd = "delete from book.productcategory where ProductId=@p0 , CategoryId=@p1";
                context.Database.ExecuteSqlRaw(cmd, productId, categoryId);
            }
        }

        public Category GetByIdWithProducts(int categoryId)
        {
            using (var context = new ShopContext())
            {
                return context.Categories
                            .Where(i => i.CategoryId == categoryId)
                            .Include(i => i.ProductCategories)
                            .ThenInclude(i => i.Product)
                            .FirstOrDefault();
            }
        }



    }
}