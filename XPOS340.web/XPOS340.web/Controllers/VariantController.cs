using Microsoft.AspNetCore.Mvc;
using XPOS240.ViewModel;
using XPOS340.web.Models;

namespace XPOS340.web.Controllers
{
    public class VariantController : Controller
    {
        private readonly VariantModel variant;
        private CategoryModel category;

        public VariantController(IConfiguration _config)
        {
            variant = new VariantModel(_config);
            category = new CategoryModel(_config);
        }

        public async Task<IActionResult> Index(string? filter)
        {
            List<VMTblMVariant>? data = await variant.GetByFilter(filter);

            ViewBag.Title = "Variant List";
            ViewBag.Filter = filter;

            return View(data);

        }
        public async Task<IActionResult> Details(int id)
        {
            VMTblMVariant? data = await variant.getById(id);

            ViewBag.Title = "Variant Detail";
            return View(data);
        }
        public async Task<IActionResult> Create()
        {
            ViewBag.Category = await category.getByFilter("");
            ViewBag.Title = "New Variant";

            return View();
        }
        [HttpPost]
        public async Task<VMResponse<VMTblMVariant>?> CreateAsync(VMTblMVariant data)
        {
            return (await variant.CreateAsync(data));
        }

        public IActionResult Delete(int id)
        {

            ViewBag.Title = "Delete Variant";

            return View(id);
        }
        [HttpPost]
        public async Task<VMResponse<VMTblMVariant>?> DeleteAsync(int id, int userId)
        {
            return (await variant.DeleteAsync(id, userId));
        }
    }
}
