using Microsoft.AspNetCore.Mvc;
using System.Net;
using XPOS240.ViewModel;
using XPOS340.web.Models;

namespace XPOS340.web.Controllers
{
    public class OrderController : Controller
    {
        private readonly ProductModel product;
        private readonly string imageFolder;
        private readonly int pageZise;
        private readonly VariantModel variant;
        private CategoryModel category;
        private OrderModel order;

        private int? custId = null;
        private int? roleId = null;
        public OrderController(IConfiguration _config, IWebHostEnvironment _webHostEnv)
        {
            product = new ProductModel(_config, _webHostEnv);
            variant = new VariantModel(_config);
            category = new CategoryModel(_config);
            order = new OrderModel(_config);
            imageFolder = _config["ImageFolder"];
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

            if (roleId != 2)
            {
                HttpContext.Session.SetString("errMsg", "you are not autor");
                return RedirectToAction("Index", "Home");
            }

            return null;
        }
        public async Task<IActionResult> Index(string? filter)
        {
            IActionResult? sessionResult = CheckSession();
            if (sessionResult != null)
            {
                return sessionResult;
            }
            List<VMTblMProduct> data = new List<VMTblMProduct>();

            try
            {
                data = (string.IsNullOrEmpty(filter))
                ? await product.getByFilter("") : await product.getByFilter(filter);
            }
            catch (Exception ex)
            {
                HttpContext.Session.SetString("errMsg", ex.Message);
            }
            ViewBag.Title = "Product Index";
            ViewBag.filter = filter;
            ViewBag.imgFolder = imageFolder;

            return View(data);
        }
        public async Task<IActionResult> IndexCopy(string? filter)
        {
            IActionResult? sessionResult = CheckSession();
            if (sessionResult != null)
            {
                return sessionResult;
            }
            List<VMTblMProduct> data = new List<VMTblMProduct>();

            try
            {
                data = (string.IsNullOrEmpty(filter))
                ? await product.getByFilter("") : await product.getByFilter(filter);
            }
            catch (Exception ex)
            {
                HttpContext.Session.SetString("errMsg", ex.Message);
            }
            ViewBag.Title = "Order Copy Index";
            ViewBag.filter = filter;
            ViewBag.imgFolder = imageFolder;

            return View(data);
        }
        public IActionResult OrderPreview()
        {
            IActionResult? sessionResult = CheckSession();
            if (sessionResult != null)
            {
                return sessionResult;
            }
            ViewBag.Title = "Order Preview";

            return View();
        }
        public IActionResult Details(List<VMTblTOrderDetail>? listCart, int totProduct, decimal estPrice)
        {
            IActionResult? sessionResult = CheckSession();
            if (sessionResult != null)
            {
                return sessionResult;
            }
            ViewBag.userId = HttpContext.Session.GetInt32("custId");
            ViewBag.Title = "Order Preview";
            ViewBag.TotProduct = totProduct;
            ViewBag.estPrice = estPrice;
            return View(listCart);
        }

        [HttpPost]
        public async Task<VMResponse<VMTblTOrder>?> SaveAsync(List<VMTblTOrderDetail>? listCart, int totProduct, decimal estPrice,int createBy)
        {
            VMTblTOrder data = new VMTblTOrder()
            {
                CustomerId = 1,
                Amount = estPrice,
                TotalQty = totProduct,
                IsCheckout = false,
                CreateBy = createBy,
                OrderDetails = listCart

            };


            return await order.SaveAsync(data);
        }
        [HttpPost]
        public async Task<VMResponse<VMTblTOrder>?> CheckOutAsync(List<VMTblTOrderDetail>? listCart, int totProduct, decimal estPrice, int createBy)
        {
            VMTblTOrder data = new VMTblTOrder()
            {
                CustomerId = 1,
                Amount = estPrice,
                TotalQty = totProduct,
                IsCheckout = true,
                CreateBy = createBy,
                OrderDetails = listCart

            };


            return await order.SaveAsync(data);
        }
        public async Task<IActionResult> History()
        {
            IActionResult? sessionResult = CheckSession();
            if (sessionResult != null)
            {
                return sessionResult;
            }
            List<VMTblTOrder> data =  await order.getByFilter("");
            ViewBag.Title = "Order Detail";

            return View(data);
        }

        [HttpPost]
        public async Task<VMResponse<VMTblTOrder>?> EditAsync (int orderHeaderId, int updateBy, bool isCheckOut = false, bool isDeleted = false)
        {
            VMTblTOrder? data = await order.GetById(orderHeaderId);
            VMResponse<VMTblTOrder>? Response = new VMResponse<VMTblTOrder>();
            if (data != null)
            {
                data.UpdateBy = updateBy;
                data.IsCheckout = isCheckOut;
                data.IsDeleted = isDeleted;

                Response = await order.UpdateAsync(data);

            }
            else
            {
                Response.statusCode = HttpStatusCode.NoContent;
                Response.message = HttpStatusCode.NoContent + "Order does Not Exit";
            }

            return Response;
        }
        public async Task<IActionResult> DetailsCheckOut2()
        {
            IActionResult? sessionResult = CheckSession();
            if (sessionResult != null)
            {
                return sessionResult;
            }

            int? userId = HttpContext.Session.GetInt32("custId");
            // Mendapatkan data dan mengurutkan berdasarkan ID (ID adalah primary key yang auto-increment)
            List<VMTblTOrder> data = await order.getByFilter(userId.ToString());

            // Ambil data terbaru (misalnya dengan mengambil data terakhir yang ditambahkan berdasarkan ID terbesar)
            VMTblTOrder latestData = data.OrderByDescending(o => o.Id).FirstOrDefault();

            ViewBag.Title = "Order Detail";

            // Jika Anda hanya ingin menampilkan data terbaru, kirimkan `latestData` ke View, bukan seluruh list
            return View(new List<VMTblTOrder> { latestData });
        }
        public async Task<IActionResult> DetailsCheckOut(int id)
        {
            IActionResult? sessionResult = CheckSession();
            if (sessionResult != null)
            {
                return sessionResult;
            }
            VMTblTOrder? data = await order.GetById(id);


            ViewBag.Title = "Order Detail";

            return View(data);
        }

    }
}
