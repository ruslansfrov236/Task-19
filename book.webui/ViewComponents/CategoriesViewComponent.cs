using System;
using System.Collections.Generic;

using System.Linq;
using System.Threading.Tasks;
using book.business.Abstract;
using Microsoft.AspNetCore.Mvc;

namespace book.webui.ViewComponents
{
    public class CategoriesViewComponent : ViewComponent
    {
         private ICategoryService _categoryService;
         
         public CategoriesViewComponent(ICategoryService categoryService ){
            this._categoryService=categoryService;
         }
        public IViewComponentResult Invoke()
        {
            if (RouteData.Values["category"]!=null)
              ViewBag.SelectedCategory= RouteData?.Values["category"];
           
              return View(_categoryService.GetAll());
        }
    }
}