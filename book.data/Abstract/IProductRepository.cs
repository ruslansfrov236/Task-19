using book.entity;

namespace book.data.Abstract
{
    public interface IProductRepository : IRepository<Product>
    {

        Product GetProductDetails(string url);
        List<Product> GetProductsByCategory(string name, int page, int pageSize);
        List<Product> GetSearchResult(string searchString);
        List<Product> GetTopFiveProducts();
        List<Product> GetHomePageProducts();
        int GetCountByCategory(string category);
        Product GetByIdWithCategories(int id);
        void Update(Product entity, int[] categoryId);

    }
}