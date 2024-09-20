using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using XPOS240.ViewModel;
using XPOS340.web.AddOns;
using XPOS340.web.Models;

namespace XPOS340.web.Controllers
{
    public class CustomerController : Controller
    {
        private readonly CustomerModel customer;
        private readonly int pageZise;
        private int? custId = null;
        private int? roleId = null;

        public CustomerController(IConfiguration _config)
        {
            customer = new CustomerModel(_config);
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
        public async Task<IActionResult> Index(string? filter, int? pageNumber, int? currPageSize)
        {
            IActionResult? sessionResult = CheckSession();
            if (sessionResult != null)
            {
                return sessionResult;
            }
            List<VMTblMCustomer>? data = new List<VMTblMCustomer>();
            try
            {
                data = (string.IsNullOrEmpty(filter)) ? await customer.getByFilter("") : await customer.getByFilter(filter);
            }
            catch (Exception ex)
            {
                HttpContext.Session.SetString("errMsg", ex.Message);
            }

            ViewBag.Title = "Customer Index";
            ViewBag.filter = filter;
            ViewBag.PageSize = (currPageSize ?? pageZise);

            return View(Pegination<VMTblMCustomer>.Create(data ?? new List<VMTblMCustomer>(), pageNumber ?? 1, ViewBag.PageSize));
        }
        public IActionResult Delete(int id)
        {

            ViewBag.Title = "Delete Customer";

            return View(id);
        }
        [HttpPost]
        public async Task<VMResponse<VMTblMCustomer>?> DeleteAsync(int id, int userId)
        {
            return (await customer.DeleteAsync(id, userId));
        }

        public async Task<IActionResult> Details(int id)
        {
            VMTblMCustomer? data = await customer.getById(id);

            ViewBag.Title = "Customer Detail";
            return View(data);
        }
        public async Task<IActionResult> Edit(int id)
        {
            VMTblMCustomer? data = await customer.getById(id);

            ViewBag.Title = "Customer Edit";
            return View(data);
        }
        [HttpPost]
        public async Task<VMResponse<VMTblMCustomer>?> EditAsync(VMTblMCustomer data)
        {
            return (await customer.UpdateAsync(data));
        }
        public IActionResult Create()
        {
            ViewBag.Title = "New Customer";

            return View();
        }
        [HttpPost]
        public async Task<VMResponse<VMTblMCustomer>?> CreateAsync(VMTblMCustomer data)
        {
            return (await customer.CreateAsync(data));
        }
    }
}
