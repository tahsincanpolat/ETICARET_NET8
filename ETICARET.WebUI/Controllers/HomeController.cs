using ETICARET.Business.Abstract;
using ETICARET.Entities;
using ETICARET.WebUI.Extensions;
using ETICARET.WebUI.Models;
using Microsoft.AspNetCore.Mvc;

namespace ETICARET.WebUI.Controllers
{
    public class HomeController : Controller
    {
        private IProductService _productService;

        public HomeController(IProductService productService)
        {
            _productService = productService;
        }

        public IActionResult Index()
        {
            var products = _productService.GetAll();

            // Boş liste kontrolü
            if (products == null || !products.Any())
            {
                products = new List<Product>();
            }

            return View(new ProductListModel() { Products = products });
        }
    }
}
