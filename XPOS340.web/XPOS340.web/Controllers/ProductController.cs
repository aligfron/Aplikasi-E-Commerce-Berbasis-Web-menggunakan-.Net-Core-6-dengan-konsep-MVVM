using Microsoft.AspNetCore.Mvc;
using XPOS240.ViewModel;
using XPOS340.web.Models;

namespace XPOS340.web.Controllers
{
    public class ProductController : Controller
    {
        private readonly ProductModel product;
        public ProductController(IConfiguration _config)
        {
            product = new ProductModel(_config);
        }
        public async Task<IActionResult> Index(string? filter)
        {
            List<VMTblMProduct>? data = (string.IsNullOrEmpty(filter)) ? await product.getByFilter("") : await product.getByFilter(filter);
            ViewBag.Title = "Product Index";
            ViewBag.filter = filter;
            return View(data);
        }
    }
}
