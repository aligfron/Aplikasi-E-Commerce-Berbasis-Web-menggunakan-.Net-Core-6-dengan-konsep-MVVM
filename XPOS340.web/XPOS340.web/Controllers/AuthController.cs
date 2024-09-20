using Microsoft.AspNetCore.Mvc;
using XPOS240.ViewModel;
using XPOS340.web.Models;

namespace XPOS340.web.Controllers
{
    public class AuthController : Controller
    {
        private CustomerModel _customer;
        public AuthController(IConfiguration configuration) {
            _customer = new CustomerModel(configuration);
        }
        public IActionResult Index()
        {
            ViewBag.Title = "Login Page";
            return View();
        }

        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(VMTblMCustomer data)
        {
            try
            {
                VMTblMCustomer? custData = await _customer.getByEmail(data.Email);
                if (custData != null)
                {
                    if(data.Password == custData.Password)
                    {
                        HttpContext.Session.SetInt32("custId", custData.Id);
                        HttpContext.Session.SetString("custName", custData.Name);
                        HttpContext.Session.SetString("custEmail", custData.Email);
                        HttpContext.Session.SetInt32("custRole", (int)custData.RoleId!);
                        HttpContext.Session.SetString("infoMsg", "Anda Berhasil Login");
                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        throw new Exception("Invalid password");
                    }
                }
                else
                {
                    throw new Exception("Customer does not exit");
                }
            }
            catch (Exception ex)
            {
                HttpContext.Session.SetString("errMsg", ex.Message);
            }
            return RedirectToAction("Index", "Home");
        }
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            HttpContext.Session.SetString("infoMsg", "Anda Berhasil Logout");
            return RedirectToAction("Index", "Home");
        }

    }
}
