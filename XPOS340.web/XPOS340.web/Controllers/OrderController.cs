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
        public OrderController(IConfiguration _config, IWebHostEnvironment _webHostEnv)
        {
            product = new ProductModel(_config, _webHostEnv);
            variant = new VariantModel(_config);
            category = new CategoryModel(_config);
            order = new OrderModel(_config);
            imageFolder = _config["ImageFolder"];
            pageZise = int.Parse(_config["PageSize"]);
        }
        public async Task<IActionResult> Index(string? filter)
        {
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
            
            ViewBag.Title = "Order Preview";

            return View();
        }
        public IActionResult Details(List<VMTblTOrderDetail>? listCart, int totProduct, decimal estPrice)
        {

            ViewBag.Title = "Order Preview";
            ViewBag.TotProduct = totProduct;
            ViewBag.estPrice = estPrice;
            return View(listCart);
        }

        [HttpPost]
        public async Task<VMResponse<VMTblTOrder>?> SaveAsync(List<VMTblTOrderDetail>? listCart, int totProduct, decimal estPrice)
        {
            VMTblTOrder data = new VMTblTOrder()
            {
                CustomerId = 1,
                Amount = estPrice,
                TotalQty = totProduct,
                IsCheckout = false,
                CreateBy = 1,
                OrderDetails = listCart

            };


            return await order.SaveAsync(data);
        }
        [HttpPost]
        public async Task<VMResponse<VMTblTOrder>?> CheckOutAsync(List<VMTblTOrderDetail>? listCart, int totProduct, decimal estPrice)
        {
            VMTblTOrder data = new VMTblTOrder()
            {
                CustomerId = 1,
                Amount = estPrice,
                TotalQty = totProduct,
                IsCheckout = true,
                CreateBy = 1,
                OrderDetails = listCart

            };


            return await order.SaveAsync(data);
        }
        public async Task<IActionResult> History()
        {
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
    }
}
