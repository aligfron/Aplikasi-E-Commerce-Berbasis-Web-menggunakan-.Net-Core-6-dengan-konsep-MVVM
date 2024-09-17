using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using XPOS240.ViewModel;
using XPOS340.web.AddOns;
using XPOS340.web.Models;

namespace XPOS340.web.Controllers
{
    public class CategoryController : Controller
    {
        private readonly CategoryModel category;
        private readonly int pageZise;
        private int? custId = null;
        private int? roleId = null;
        public CategoryController(IConfiguration _config) {
            category = new CategoryModel(_config);
            pageZise = int.Parse(_config["PageSize"]);


            
        }
        private IActionResult? CheckSession()
        {
            int? custId = HttpContext.Session.GetInt32("custId");
            int? roleId = HttpContext.Session.GetInt32("custRole");

            if (custId == null)
            {
                HttpContext.Session.SetString("errMsg", "Please Login first");
                return RedirectToAction("Index", "Auth");
            }

            if (roleId != 1)
            {
                HttpContext.Session.SetString("errMsg", "you are not autor");
                return RedirectToAction("Index", "Auth");
            }

            return null;
        }

        public async Task <IActionResult> Index(string? filter, int? pageNumber, int? currPageSize)
        {

            IActionResult? sessionResult = CheckSession();
            if (sessionResult != null)
            {
                return sessionResult;
            }
            List<VMTblMCategory>? data = new List<VMTblMCategory>();
            try
            {
                data = (string.IsNullOrEmpty(filter)) ? await category.getByFilter("") : await category.getByFilter(filter);

            }
            catch (Exception ex)
            {
                HttpContext.Session.SetString("errMsg", ex.Message);
            }


            ViewBag.Title = "Category Index";
            ViewBag.filter = filter;
            ViewBag.PageSize = (currPageSize ?? pageZise);

            return View(Pegination<VMTblMCategory>.Create(data ?? new List<VMTblMCategory>(), pageNumber ?? 1, ViewBag.PageSize));
        }
        public async Task<IActionResult> Details(int id)
        {
            CheckSession();
            VMTblMCategory? data = await category.getById(id);

            ViewBag.Title = "Category Detail";
            return View(data);
        }
        public async Task<IActionResult> Edit(int id)
        {
            CheckSession();
            VMTblMCategory? data = await category.getById(id);

            ViewBag.Title = "Category Edit";
            return View(data);
        }
        [HttpPost]
        public async Task<VMResponse<VMTblMCategory>?> EditAsync(VMTblMCategory data)
        {
            CheckSession();
            return (await category.UpdateAsync(data));
        }
        public IActionResult Create()
        {
            CheckSession();
            ViewBag.Title = "New Category";

            return View();
        }
        [HttpPost]
        public async Task<VMResponse<VMTblMCategory>?> CreateAsync(VMTblMCategory data)
        {
            CheckSession();
            return (await category.CreateAsync(data));
        }
        public IActionResult Delete(int id)
        {
            CheckSession();
            ViewBag.Title = "New Category";

            return View(id);
        }
        [HttpPost]
        public async Task<VMResponse<VMTblMCategory>?> DeleteAsync(int id, int userId)
        {
            CheckSession();
            return (await category.DeleteAsync(id,userId));
        }
    }
}
