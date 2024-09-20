using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using XPOS240.ViewModel;
using XPOS340.web.Models;

namespace XPOS340.web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly HomeModel home;

        public HomeController(ILogger<HomeController> logger, IConfiguration _config)
        {
           
            _logger = logger;
            home = new HomeModel(_config);
        }

        public IActionResult Index()
        {
            //return View();
           List <VMTblCoba>? datacoba = null;
            try
            {
                datacoba = home.getAllCoba();
            }
            catch (Exception ex)
            {
                HttpContext.Session.SetString("errMsg", ex.Message);
            }
            return View(datacoba);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
