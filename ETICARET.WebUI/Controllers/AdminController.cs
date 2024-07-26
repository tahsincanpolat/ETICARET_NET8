using ETICARET.Business.Abstract;
using ETICARET.Entities;
using ETICARET.WebUI.Identity;
using ETICARET.WebUI.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ETICARET.WebUI.Controllers
{
    public class AdminController : Controller
    {
        private IProductService _productService;
        private ICategoryService _categoryService;
        private UserManager<ApplicationUser> _userManager;
        private RoleManager<IdentityRole> _roleManager;

        public AdminController(IProductService productService, ICategoryService categoryService, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _productService = productService;
            _categoryService = categoryService;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        [Route("admin/products")]
        [ApiExplorerSettings(IgnoreApi = true)]
        public IActionResult ProductList()
        {
            return View(
                new ProductListModel()
                {
                    Products = _productService.GetAll()
                }
            );
        }

        public IActionResult CreateProduct()
        {
            var category = _categoryService.GetAll();
            ViewBag.Category = category.Select(x => new SelectListItem { Text = x.Name, Value = x.Id.ToString() });

            return View(new ProductModel());
        }
        [HttpPost]
        public async Task<IActionResult> CreateProduct(ProductModel model, List<IFormFile> files)
        {
            ModelState.Remove("SelectedCategories");

            if (ModelState.IsValid)
            {
                if (int.Parse(model.CategoryId) == -1)
                {
                    ModelState.AddModelError("CategoryID", "Lütfen Kategori seçiniz");
                    ViewBag.Category = _categoryService.GetAll().Select(x => new SelectListItem { Text = x.Name, Value = x.Id.ToString() });

                    return View(model);
                }

                var entity = new Product()
                {
                    Name = model.Name,
                    Description = model.Description,
                    Price = model.Price
                };

                if (files != null)
                {
                    foreach (var file in files)
                    {
                        Image image = new Image();
                        image.ImageUrl = file.FileName;

                        entity.Images.Add(image);

                        var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\img", file.FileName);

                        using (var stream = new FileStream(path, FileMode.Create))
                        {
                            await file.CopyToAsync(stream);
                        }
                    }
                }


                entity.ProductCategories = new List<ProductCategory> { new ProductCategory() { CategoryId = int.Parse(model.CategoryId), ProductId = entity.Id } };

                _productService.Create(entity);

                return Redirect("/admin/products");
            }

            ViewBag.Category = _categoryService.GetAll().Select(x => new SelectListItem { Text = x.Name, Value = x.Id.ToString() });

            return View(model);
        }


        public IActionResult EditProduct(int id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var entity = _productService.GetProductDetail(id);

            if (entity == null)
            {
                return NotFound();
            }

            var model = new ProductModel()
            {
                Id = entity.Id,
                Name = entity.Name,
                Description = entity.Description,
                Price = entity.Price,
                Images = entity.Images,
                SelectedCategories = entity.ProductCategories.Select(i => i.Category).ToList()
            };

            ViewBag.Categories = _categoryService.GetAll();

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> EditProduct(ProductModel model, List<IFormFile> files, int[] categoryIds)
        {

            var entity = _productService.GetById(model.Id);

            if (entity == null)
            {
                return NotFound();
            }

            entity.Name = model.Name;
            entity.Description = model.Description;
            entity.Price = model.Price;


            if (files != null)
            {
                foreach (var file in files)
                {
                    Image image = new Image();
                    image.ImageUrl = file.FileName;

                    entity.Images.Add(image);

                    var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\img", file.FileName);

                    using (var stream = new FileStream(path, FileMode.Create))
                    {
                        await file.CopyToAsync(stream);
                    }
                }

                _productService.Update(entity,categoryIds);

                return Redirect("/admin/products");
            }

            return View(entity);


        }

        public IActionResult DeleteProduct(int productId)
        {
            var product = _productService.GetById(productId);

            if (product != null) { 
                _productService.Delete(product);            
            }

            return RedirectToAction("ProductList");
        }

        public IActionResult CategoryList()
        {
            return View(new CategoryListModel() { Categories = _categoryService.GetAll() });
        }

        public IActionResult EditCategory(int? id)
        {
            var entity = _categoryService.GetByWithProducts(id.Value);

            return View(
                new CategoryModel()
                {
                    Id = entity.Id,
                    Name = entity.Name,
                    Products = entity.ProductCategories.Select(i => i.Product).ToList()
                }
            );
        }

        [HttpPost]
        public IActionResult EditCategory(CategoryModel model)
        {
            var entity = _categoryService.GetById(model.Id);

            if(entity == null)
            {
                return NotFound();
            }

            entity.Name = model.Name;
            _categoryService.Update(entity);

            return RedirectToAction("CategoryList");
        }

        [HttpPost]
        public IActionResult DeleteFromCategory(int categoryId,int productId)
        {
            _categoryService.DeleteFromCategory(categoryId, productId);
            return Redirect("/admin/categories/"+ categoryId);
        }

        [HttpPost]
        public IActionResult DeleteCategory(int categoryId)
        {
            var entity = _categoryService.GetById(categoryId);

            _categoryService.Delete(entity);

            return RedirectToAction("CategoryList");
        }


        public IActionResult CreateCategory()
        {
            return View(new CategoryModel());
        }


        [HttpPost]
        public IActionResult CreateCategory(CategoryModel model)
        {
            var entity = new Category()
            {
                Name = model.Name
            };
            _categoryService.Create(entity);

            return RedirectToAction("CategoryList");
        }
    }
}
