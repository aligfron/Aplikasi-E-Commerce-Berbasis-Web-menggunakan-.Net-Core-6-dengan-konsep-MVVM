using Microsoft.AspNetCore.Mvc;
using XPOS240.ViewModel;
using XPOS340.web.Models;

namespace XPOS340.web.Controllers
{
    public class VariantController : Controller
    {
        private readonly VariantModel variant;
        public VariantController(IConfiguration _config)
        {
            variant = new VariantModel(_config);
        }
        public async Task<IActionResult> Index(string? filter)
        {
            List<VMTblMVariant>? data = (string.IsNullOrEmpty(filter)) ? await variant.getByFilter("") : await variant.getByFilter(filter);
            ViewBag.Title = "Variant Index";
            ViewBag.filter = filter;
            return View(data);
        }
    }
}
