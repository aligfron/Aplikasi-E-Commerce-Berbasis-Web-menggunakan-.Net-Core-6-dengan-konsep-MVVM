using Microsoft.AspNetCore.Mvc;
using XPOS240.ViewModel;
using XPOS340.web.Models;

namespace XPOS340.web.Controllers
{
    public class CustomerController : Controller
    {
        private readonly CustomerModel customer;
        public CustomerController(IConfiguration _config)
        {
            customer = new CustomerModel(_config);
        }
        public async Task<IActionResult> Index(string? filter)
        {
            List<VMTblMCustomer>? data = (string.IsNullOrEmpty(filter)) ? await customer.getByFilter("") : await customer.getByFilter(filter);
            ViewBag.Title = "Customer Index";
            ViewBag.filter = filter;
            return View(data);
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
    }
}
