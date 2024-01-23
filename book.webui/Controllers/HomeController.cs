using book.business.Abstract;
using book.webui.Models;
using Microsoft.AspNetCore.Mvc;

namespace book.webui.Controllers
{
    public class HomeController : Controller
    {
        private IProductService _productService;


        public HomeController(IProductService productService)
        {
            this._productService = productService;
        }
        public IActionResult Index()
        {



            var productviewmodel = new ProductListViewModel()
            {
                Products = _productService.GetHomePageProducts()

            };
            return View(productviewmodel);
        }
        public IActionResult Contact()
        {
            return View();
        }
    }
}