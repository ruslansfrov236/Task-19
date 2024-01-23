using book.entity;
using book.business.Abstract;
using book.webui.Models;
using Microsoft.AspNetCore.Mvc;
namespace book.webui.Controllers
{
    public class BookController : Controller
    {
        private IProductService _productService;


        public BookController(IProductService productService)
        {
            this._productService = productService;
        }
        public IActionResult List(string category, int page = 1)
        {

            const int pageSize = 4;

            var productviewmodel = new ProductListViewModel()
            {
                PageInfo = new PageInfo()
                {
                    TotalItems = _productService.GetCountByCategory(category),
                    CurrentPage = page,
                    ItemsPerPage = pageSize,
                    CurrentCategory = category
                },
                Products = _productService.GetProductsByCategory(category, page, pageSize)

            };
            return View(productviewmodel);
        }
        public IActionResult Create()
        {
            return View();
        }

        public IActionResult Details(string url)
        {
            if (url == null)
            {
                return NotFound();
            }
            Product product = _productService.GetProductDetails(url);
            if (product == null)
            {
                return NotFound();
            }

            return View(new ProductDetailModel
            {
                Product = product,
                Categories = product.ProductCategories.Select(i => i.Category).ToList()
            });


        }
        public IActionResult Search(string search)
        {
            var productViewModel = new ProductListViewModel()
            {
                
                Products = _productService.GetSearchResult(search)
            };

            return View(productViewModel);
        }
    }
}