using Microsoft.AspNetCore.Mvc;
using XPOS240.ViewModel;
using XPOS340.web.Models;

namespace XPOS340.web.Controllers
{
    public class ProductController : Controller
    {
        private readonly ProductModel product;
        private readonly string imageFolder;

        private readonly VariantModel variant;
        private CategoryModel category;
        public ProductController(IConfiguration _config, IWebHostEnvironment _webHostEnv)
        {
            product = new ProductModel(_config, _webHostEnv);
            variant = new VariantModel(_config);
            category = new CategoryModel(_config);
            imageFolder = _config["ImageFolder"];
        }
        public async Task<IActionResult> Index(string? filter)
        {
            List<VMTblMProduct>? data = (string.IsNullOrEmpty(filter)) 
                ? await product.getByFilter("") : await product.getByFilter(filter);
            ViewBag.Title = "Product Index";
            ViewBag.filter = filter;
            ViewBag.imgFolder = imageFolder;
            return View(data);
        }
        public async Task<IActionResult> Details(int id)
        {
            VMTblMProduct? data = await product.getById(id);

            ViewBag.imgFolder = imageFolder;
            ViewBag.Title = "Product Detail";
            return View(data);
        }
        public async Task<IActionResult> Create()
        {
            ViewBag.Category = await category.getByFilter("");
            ViewBag.Variant = await variant.GetByFilter("");
            ViewBag.Title = "New Product";

            return View();
        }
        [HttpPost]
        public async Task<VMResponse<VMTblMProduct>?> CreateAsync(VMTblMProduct data)
        {
            return (await product.CreateAsync(data));
        }
        public async Task<VMResponse<List<VMTblMVariant>>?> GetVariantByCategory(int categoryId)
        {
            return (await variant.getByCategory(categoryId));
        }
        
        public async Task<IActionResult> Edit(int id)
        {
            VMTblMProduct? data = await product.getById(id);
            ViewBag.Category = await category.getByFilter("");
            ViewBag.imgFolder = imageFolder;
            ViewBag.Title = "Edit Product";
            return View(data);
        }
        [HttpPost]
        public async Task<VMResponse<VMTblMProduct>> EditAsync(VMTblMProduct data)
        {
            return (await product.UpdateAsync(data));
        }
        public IActionResult Delete(int id)
        {

            ViewBag.Title = "Delete Product";

            return View(id);
        }
        [HttpPost]
        public async Task<VMResponse<VMTblMProduct>?> DeleteAsync(int id, int userId)
        {
            return (await product.DeleteAsync(id, userId));
        }
    }
}
