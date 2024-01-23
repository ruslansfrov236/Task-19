using System;
using System.Threading.Tasks;
using book.business.Abstract;
using book.webui.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace book.webui.Controllers
{
    [Authorize]
    public class AdminController : Controller
    {
        private IProductService _productService;
        private ICategoryService _categoryService;

        public AdminController(IProductService productService, ICategoryService categoryService)
        {
            this._productService = productService;
            this._categoryService = categoryService;
        }


        public IActionResult ProductList()
        {
            return ProductList(_productService);
        }
        [HttpPost]
        public IActionResult ProductList(IProductService _productService)
        {
            return View(new ProductListViewModel()
            {
                Products = _productService.GetAll()
            });
        }

        public IActionResult CreateProduct()
        {
            return View();
        }
        [HttpPost]
        public IActionResult CreateProduct(ProductModel model)
        {
            if (ModelState.IsValid)
            {
                var entity = new entity.Product()
                {
                    Name = model.Name,
                    Url = model.Url,
                    Price = (decimal?)model.Price,
                    Description = model.Description,
                    ImageUrl = model.ImageUrl
                };

                if (_productService.Create(entity))
                {
                    CreateMessage("qeyd elave olundu", "success");
                    return RedirectToAction("ProductList");
                }
                CreateMessage(_productService.ErrorMessage, "danger");
                return View(model);
            }
            return View(model);


        }
        public IActionResult Edit(int? id)
        {

            if (id == null)
            {
                return NotFound();
            }
            var entity = _productService.GetByIdWithCategories((int)id);
            if (entity == null)
            {
                return NotFound();
            }

            var model = new ProductModel()
            {
                ProductId = entity.ProductId,
                Name = entity.Name,

                Url = entity.Url,
                Price = (double?)entity.Price,
                Description = entity.Description,
                ImageUrl = entity.ImageUrl,
                IsApproved = entity.IsApproved,
                IsHome = entity.IsHome,
                SelectedCategories = entity.ProductCategories.Select(i => i.Category).ToList()
            };
            @ViewBag.Categories = _categoryService.GetAll();
            return View(model);


        }
        [HttpPost]
        public async System.Threading.Tasks.Task<IActionResult> EditAsync(ProductModel model, int[] categoryIds, IFormFile file)
        {
            if (ModelState.IsValid)
            {
                var entity = _productService.GetById(model.ProductId);
                if (entity == null)
                {
                    return NotFound();
                }
                entity.Name = model.Name;
                entity.Price = (decimal?)model.Price;
                entity.Url = model.Url;
                entity.Description = model.Description;
                entity.IsApproved = model.IsApproved;
                entity.IsHome = model.IsHome;


                if (file != null)
                {

                    var extention = Path.GetExtension(file.FileName);
                    var randomName = string.Format($"{Guid.NewGuid()}{extention}");
                    entity.ImageUrl = randomName;
                    var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\img", randomName);

                    using (var stream = new FileStream(path, FileMode.Create))
                    {
                        await file.CopyToAsync(stream);
                    }

                }
                if (_productService.Update(entity, categoryIds))
                {
                    CreateMessage("qeyd guncelendi ", "success");

                    return RedirectToAction("ProductList");
                };
                CreateMessage(_productService.ErrorMessage, "danger");
            }
            ViewBag.Categories = _categoryService.GetAll();
            return View(model);
        }


        public IActionResult ProductDelete(int productId)
        {

            var entity = _productService.GetById(productId);
            if (entity != null)
            {
                _productService.Delete(entity);
            }
            var msg = new AlertMessage()
            {
                Message = $"{entity.Name} adli mehsul silindi.",
                AlertType = "danger"
            };

            TempData["message"] = JsonConvert.SerializeObject(msg);

            return RedirectToAction("ProductList");
        }


        /// Category/////
        public IActionResult CategoryList()
        {

            return View(new CategoryListViewModel()
            {
                Categories = _categoryService.GetAll()
            });
        }


        public IActionResult CategoryEdit(int? id)
        {

            if (id == null)
            {
                return NotFound();
            }
            var entity = _categoryService.GetByIdWithProducts((int)id);
            if (entity == null)
            {
                return NotFound();
            }

            var model = new CategoryModel()
            {
                CategoryId = entity.CategoryId,
                Name = entity.Name,
                Url = entity.Url,
                Products = entity.ProductCategories.Select(p => p.Product)
                                                  .ToList()



            };

            return View(model);


        }
        [HttpPost]
        public IActionResult CategoryEdit(CategoryModel model)
        {
            if (ModelState.IsValid)
            {
                var entity = _categoryService.GetById(model.CategoryId);
                if (entity == null)
                {
                    return NotFound();
                }
                entity.Name = model.Name;
                entity.Url = model.Url;


                _categoryService.Update(entity);
                var msg = new AlertMessage()
                {
                    Message = $"{entity.Name} adli category redakte etdiniz.",
                    AlertType = "success"
                };
                TempData["message"] = JsonConvert.SerializeObject(msg);
                return RedirectToAction("CategoryList");
            }
            return View(model);

        }



        public IActionResult CategoryCreate()
        {
            return View();
        }
        [HttpPost]
        public IActionResult CategoryCreate(CategoryModel model)
        {
            if (ModelState.IsValid)
            {
                var entity = new entity.Category()
                {
                    Name = model.Name,
                    Url = model.Url,

                };

                _categoryService.Create(entity);


                return RedirectToAction("CategoryList");
            }
            return View(model);


        }
        public IActionResult DeleteCategory(int categoryId)
        {
            var entity = _categoryService.GetById(categoryId);

            if (entity != null)
            {
                _categoryService.Delete(entity);
            }

            var msg = new AlertMessage()
            {
                Message = $"{entity.Name} isimli category silindi.",
                AlertType = "danger"
            };

            TempData["message"] = JsonConvert.SerializeObject(msg);

            return RedirectToAction("CategoryList");
        }

        [HttpPost]
        public IActionResult DeleteFromCategory(int productId, int categoryId)
        {
            _categoryService.DeleteFromCategory(productId, categoryId);
            return Redirect("/admin/category/" + categoryId);

        }

        private void CreateMessage(string message, string alerttype)
        {
            var msg = new AlertMessage()
            {
                Message = message,
                AlertType = alerttype
            };
            TempData["message"] = JsonConvert.SerializeObject(msg);
            //  TempData["message"]= $"{entity.Name} adli category elave olundu.";
        }



    }
}