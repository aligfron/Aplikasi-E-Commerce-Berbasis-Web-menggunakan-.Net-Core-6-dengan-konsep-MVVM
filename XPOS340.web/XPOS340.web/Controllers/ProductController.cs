using Microsoft.AspNetCore.Mvc;
using XPOS240.ViewModel;
using XPOS340.web.AddOns;
using XPOS340.web.Models;

namespace XPOS340.web.Controllers
{
    public class ProductController : Controller
    {
        private readonly ProductModel product;
        private readonly string imageFolder;
        private readonly int pageZise;
        private readonly VariantModel variant;
        private CategoryModel category;
        private int? custId = null;
        private int? roleId = null;
        public ProductController(IConfiguration _config, IWebHostEnvironment _webHostEnv)
        {
            product = new ProductModel(_config, _webHostEnv);
            variant = new VariantModel(_config);
            category = new CategoryModel(_config);
            imageFolder = _config["ImageFolder"];
            pageZise = int.Parse(_config["PageSize"]);
        }
        private IActionResult? CheckSession()
        {
            int? custId = HttpContext.Session.GetInt32("custId");
            int? roleId = HttpContext.Session.GetInt32("custRole");

            if (custId == null)
            {
                HttpContext.Session.SetString("errMsg", "Please Login first");
                return RedirectToAction("Index", "Home");
            }

            if (roleId != 1)
            {
                HttpContext.Session.SetString("errMsg", "you are not autor");
                return RedirectToAction("Index", "Home");
            }

            return null;
        }
        public async Task<IActionResult> Index(string? filter, int? pageNumber, int? currPageSize, string? orderBy)
        {
            IActionResult? sessionResult = CheckSession();
            if (sessionResult != null)
            {
                return sessionResult;
            }
            List<VMTblMProduct>? data = (string.IsNullOrEmpty(filter)) 
                ? await product.getByFilter("") : await product.getByFilter(filter);

            switch (orderBy)
            {
                case "name_desc":
                    data = data?.OrderByDescending(p => p.Name).ToList();
                    break;
                case "name":
                    data = data?.OrderBy(p => p.Name).ToList();
                    break;
                case "price_desc":
                    data = data?.OrderByDescending(p => p.Price).ToList();
                    break;
                case "price":
                    data = data?.OrderBy(p => p.Price).ToList();
                    break;
                case "stock_desc":
                    data = data?.OrderByDescending(p => p.Stock).ToList();
                    break;
                case "stock":
                    data = data?.OrderBy(p => p.Stock).ToList();
                    break;
                case "Category_desc":
                    data = data?.OrderByDescending(p => p.CategoryName).ToList();
                    break;
                case "category":
                    data = data?.OrderBy(p => p.CategoryName).ToList();
                    break;
                case "variant_desc":
                    data = data?.OrderByDescending(p => p.VariantName).ToList();
                    break;
                case "variant":
                    data = data?.OrderBy(p => p.VariantName).ToList();
                    break;
                default:
                    data = data?.OrderBy(p => p.Id).ToList();
                    break;
            }

            ViewBag.OrderId = string.IsNullOrEmpty(orderBy) ? "id_desc" : "";
            ViewBag.OrderName = (orderBy == "name") ? "name_desc" : "name";
            ViewBag.OrderPrice = (orderBy == "price") ? "price_desc" : "price";
            ViewBag.OrderStock = (orderBy == "stock") ? "stock_desc" : "stock";
            ViewBag.OrderCategory = (orderBy == "category") ? "category_desc" : "category";
            ViewBag.OrderVariant = (orderBy == "variant") ? "variant_desc" : "variant";
            ViewBag.Title = "Product Index";
            ViewBag.filter = filter;
            ViewBag.imgFolder = imageFolder;
            
            ViewBag.OrderBy = orderBy;
            ViewBag.PageSize = (currPageSize ?? pageZise);

            return View(Pegination<VMTblMProduct>.Create(data ?? new List<VMTblMProduct>(), pageNumber ?? 1, ViewBag.PageSize));
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
