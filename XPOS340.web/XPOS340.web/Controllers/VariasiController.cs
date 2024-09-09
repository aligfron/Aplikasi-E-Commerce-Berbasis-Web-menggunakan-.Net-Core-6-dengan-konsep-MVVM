using Microsoft.AspNetCore.Mvc;
using XPOS240.ViewModel;
using XPOS340.web.Models;

namespace XPOS340.web.Controllers
{
    public class VariasiController : Controller
    {
        private readonly VariasiModel variasi;
        public VariasiController(IConfiguration _config)
        {
            variasi = new VariasiModel(_config);
        }
        public async Task<IActionResult> Index(string? filter)
        {
            List<VMTblMVariant>? data = (string.IsNullOrEmpty(filter)) 
                ? await variasi.getByFilter("") : await variasi.getByFilter(filter);
            ViewBag.Title = "Variant Index";
            ViewBag.filter = filter;
            return View(data);
        }
    }
}
