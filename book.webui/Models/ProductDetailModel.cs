using book.entity;

namespace book.webui.Models
{
    public class ProductDetailModel
    {
        public Product? Product{get; set;}
        public  List<Category>? Categories {get; set;}
    }
}