using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using XPOS240.ViewModel;
using XPOS340.web.AddOns;
using XPOS340.web.Models;

namespace XPOS340.web.Controllers
{
    public class VariantController : Controller
    {
        private readonly VariantModel variant;
        private CategoryModel category;
        private readonly int pageZise;

        public VariantController(IConfiguration _config)
        {
            variant = new VariantModel(_config);
            category = new CategoryModel(_config);
            pageZise = int.Parse(_config["PageSize"]);
        }

        public async Task<IActionResult> Index(string? filter, int? pageNumber, int? currPageSize)
        {
            List<VMTblMVariant>? data = await variant.GetByFilter(filter);

            ViewBag.Title = "Variant List";
            ViewBag.Filter = filter;

            ViewBag.PageSize = (currPageSize ?? pageZise);

            return View(Pegination<VMTblMVariant>.Create(data ?? new List<VMTblMVariant>(), pageNumber ?? 1, ViewBag.PageSize));

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
        public async Task<IActionResult> Edit(int id)
        {
            VMTblMVariant? data = await variant.getById(id);
            ViewBag.Category = await category.getByFilter("");

            ViewBag.Title = "Variant Edit";
            return View(data);
        }
        [HttpPost]
        public async Task<VMResponse<VMTblMVariant>?> EditAsync(VMTblMVariant data)
        {
            return (await variant.UpdateAsync(data));
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
