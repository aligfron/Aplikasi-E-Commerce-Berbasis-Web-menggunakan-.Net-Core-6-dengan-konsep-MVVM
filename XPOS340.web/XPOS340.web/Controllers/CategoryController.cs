using Microsoft.AspNetCore.Mvc;
using XPOS240.ViewModel;
using XPOS340.web.Models;

namespace XPOS340.web.Controllers
{
    public class CategoryController : Controller
    {
        private readonly CategoryModel category;
        public CategoryController(IConfiguration _config) {
            category = new CategoryModel(_config);
        }
        public async Task <IActionResult> Index(string? filter)
        {
            List<VMTblMCategory>? data = (string.IsNullOrEmpty(filter)) ? await category.getByFilter("") : await category.getByFilter(filter);
            
            ViewBag.Title = "Category Index";
            ViewBag.filter = filter;
            return View(data);
        }
        public async Task<IActionResult> Details(int id)
        {
            VMTblMCategory? data = await category.getById(id);

            ViewBag.Title = "Category Detail";
            return View(data);
        }
        public async Task<IActionResult> Edit(int id)
        {
            VMTblMCategory? data = await category.getById(id);

            ViewBag.Title = "Category Edit";
            return View(data);
        }
        [HttpPost]
        public async Task<VMResponse<VMTblMCategory>?> EditAsync(VMTblMCategory data)
        {
            return (await category.UpdateAsync(data));
        }
        public IActionResult Create()
        {
            ViewBag.Title = "New Category";

            return View();
        }
        [HttpPost]
        public async Task<VMResponse<VMTblMCategory>?> CreateAsync(VMTblMCategory data)
        {
            return (await category.CreateAsync(data));
        }
        public IActionResult Delete(int id)
        {

            ViewBag.Title = "New Category";

            return View(id);
        }
        [HttpPost]
        public async Task<VMResponse<VMTblMCategory>?> DeleteAsync(int id, int userId)
        {
            return (await category.DeleteAsync(id,userId));
        }
    }
}
