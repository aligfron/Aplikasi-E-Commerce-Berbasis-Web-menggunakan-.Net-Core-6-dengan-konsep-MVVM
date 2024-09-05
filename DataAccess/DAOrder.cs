using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using XPOS240.ViewModel;
using XPOS340.DataModel;

namespace DataAccess
{
    public class DAOrder
    {
        private XPOS340Context db;
        private string TrxCodeGenerator(int? lastOrderId)
        {
            string trxcode = $"XA-{DateTime.Now.ToString("ddMMyyyy")}-";

            string trxNo = (lastOrderId != null && lastOrderId > 0)
                ? (lastOrderId.Value + 1).ToString("D5")
                : "00001";

            trxcode += trxNo;

            return trxcode;
        }
        public DAOrder(XPOS340Context _db)
        {
            db = _db;
        }


        public VMResponse<VMTblTOrder> GetById(int id)
        {

            VMResponse<VMTblTOrder> response = new VMResponse<VMTblTOrder>();
            try
            {
                response.data = (
                    from oh in db.TblTOrderHeaders
                    join od in db.TblTOrderDetails
                    on oh.Id equals od.OrderHeaderId
                    join c in db.TblMCustomers
                    on oh.CustomerId equals c.Id
                    where oh.IsDeleted == false && oh.Id == id
                    select new VMTblTOrder
                    {
                        Id = oh.Id,
                        TrxCode = oh.TrxCode,

                        CustomerId = oh.CustomerId,
                        CustomerName = c.Name,

                        Amount = oh.Amount,
                        TotalQty = oh.TotalQty,
                        IsCheckout = oh.IsCheckout,
                        CreateBy = oh.CreateBy,
                        CreateDate = oh.CreateDate,
                        UpdateBy = oh.UpdateBy,
                        UpdateDate = oh.UpdateDate,




                        OrderDetails = (
                            from od in db.TblTOrderDetails
                            join p in db.TblMProducts on od.ProductId equals p.Id
                            where od.IsDeleted == false && od.OrderHeaderId == oh.Id
                            select new VMTblTOrderDetail
                            {
                                Id = od.Id,
                                OrderHeaderId = od.OrderHeaderId,
                                Price = od.Price,
                                Qty = od.Qty,

                                ProductId = p.Id,
                                ProductName = p.Name,

                                IsDeleted = od.IsDeleted,

                                CreateBy = od.CreateBy,
                                CreateDate = od.CreateDate,
                                UpdateBy = od.UpdateBy,
                                UpdateDate = od.UpdateDate

                            }
                        ).ToList()
                    }

                    ).FirstOrDefault();
                if (response.data != null)
                {
                    response.statusCode = HttpStatusCode.OK;
                    response.message = $"{HttpStatusCode.OK} - Order Data Has been Sucsesfully";
                }
                else
                {
                    response.statusCode = HttpStatusCode.OK;
                    response.message = $"{HttpStatusCode.OK} - Order Data does not exis";
                }
            }
            catch (Exception ex)
            {
                response.message = $"{HttpStatusCode.InternalServerError}" + ex.Message;
            }
            return response;
        }
        public VMResponse<List<VMTblTOrder>> GetByFilter(string filter)
        {
            VMResponse<List<VMTblTOrder>> response = new VMResponse<List<VMTblTOrder>>();
            try
            {
                response.data = (
                    from oh in db.TblTOrderHeaders
                    join c in db.TblMCustomers on oh.CustomerId equals c.Id
                    where oh.IsDeleted == false
                          && (oh.TrxCode.Contains(filter) ||
                              c.Name.Contains(filter) ||
                              oh.Amount.ToString().Contains(filter) ||
                              oh.TotalQty.ToString().Contains(filter))
                    select new VMTblTOrder
                    {
                        Id = oh.Id,
                        TrxCode = oh.TrxCode,
                        CustomerId = oh.CustomerId,
                        CustomerName = c.Name,
                        Amount = oh.Amount,
                        TotalQty = oh.TotalQty,
                        IsCheckout = oh.IsCheckout,
                        CreateBy = oh.CreateBy,
                        CreateDate = oh.CreateDate,
                        UpdateBy = oh.UpdateBy,
                        UpdateDate = oh.UpdateDate,
                        OrderDetails = (
                            from od in db.TblTOrderDetails
                            join p in db.TblMProducts on od.ProductId equals p.Id
                            where od.IsDeleted == false && od.OrderHeaderId == oh.Id
                            select new VMTblTOrderDetail
                            {
                                Id = od.Id,
                                OrderHeaderId = od.OrderHeaderId,
                                Price = od.Price,
                                Qty = od.Qty,
                                ProductId = p.Id,
                                ProductName = p.Name,
                                IsDeleted = od.IsDeleted,
                                CreateBy = od.CreateBy,
                                CreateDate = od.CreateDate,
                                UpdateBy = od.UpdateBy,
                                UpdateDate = od.UpdateDate
                            }
                        ).ToList()
                    }
                ).ToList();

                if (response.data.Count > 0)
                {
                    response.statusCode = HttpStatusCode.OK;
                    response.message = $"{HttpStatusCode.OK} - {response.data.Count} Order(s) found successfully.";
                }
                else
                {
                    response.statusCode = HttpStatusCode.NoContent;
                    response.message = $"{HttpStatusCode.NoContent} - No orders found.";
                }
            }
            catch (Exception ex)
            {
                response.statusCode = HttpStatusCode.InternalServerError;
                response.message = $"{HttpStatusCode.InternalServerError} - {ex.Message}";
            }

            return response;
        }


        public VMResponse<VMTblTOrder> Create(VMTblTOrder data)
        {
            VMResponse<VMTblTOrder> response = new VMResponse<VMTblTOrder>();
            using (IDbContextTransaction dbTrans = db.Database.BeginTransaction())
            {
                try
                {
                    TblTOrderHeader orderHeader = new TblTOrderHeader();
                    TblTOrderDetail orderDetail = new TblTOrderDetail();
                    TblMProduct product = new TblMProduct();

                    orderHeader.TrxCode = TrxCodeGenerator(
                        db.TblTOrderHeaders.OrderBy(h => h.Id).Select(h => h.Id).LastOrDefault());

                    orderHeader.Amount = data.Amount;
                    orderHeader.TotalQty = data.TotalQty;
                    orderHeader.CustomerId = data.CustomerId;
                    orderHeader.IsCheckout = data.IsCheckout;
                    orderHeader.CreateBy = data.CreateBy;

                    db.Add(orderHeader);
                    db.SaveChanges();


                    if (data.OrderDetails != null)
                    {
                        foreach (VMTblTOrderDetail item in data.OrderDetails)
                        {
                            orderDetail = new TblTOrderDetail();
                            orderDetail.OrderHeaderId = orderHeader.Id;
                            orderDetail.ProductId = item.ProductId;
                            orderDetail.Qty = item.Qty;
                            orderDetail.Price = item.Price;

                            orderDetail.CreateBy = orderHeader.CreateBy;
                            orderDetail.CreateDate = orderHeader.CreateDate;

                            db.Add(orderDetail);
                            db.SaveChanges();


                            //update product stuck

                            product = db.TblMProducts.Find(item.ProductId)!;

                            if (product.Stock >= item.Qty)
                            {
                                product.Stock -= item.Qty;

                                product.UpdateBy = orderHeader.CreateBy;
                                product.UpdateDate = orderHeader.CreateDate;


                                db.Update(product);
                                db.SaveChanges();

                            }
                            else
                            {
                                response.statusCode = HttpStatusCode.BadRequest;
                                throw new Exception($"Product {item.ProductName} does not have stock");
                            }

                        }
                    }
                    dbTrans.Commit();
                    response.data = data;
                    response.statusCode = HttpStatusCode.Created;
                    response.message = $"{HttpStatusCode.Created} = new order has been suksesfully";

                }
                catch (Exception ex)
                {
                    dbTrans.Rollback();
                    response.message = $"{HttpStatusCode.InternalServerError}" + ex.Message;
                }
            }
            return response;
        }

        public VMResponse<VMTblTOrder> Update(VMTblTOrder newData)
        {
            VMResponse<VMTblTOrder> response = new VMResponse<VMTblTOrder>();
            using (IDbContextTransaction dbTrans = db.Database.BeginTransaction())
            {

                try
                {
                    TblTOrderHeader? existingHeader = db.TblTOrderHeaders.Find(newData.Id);
                    TblTOrderDetail? existingdetail;
                    TblMProduct? existingProduct;
                    if (existingHeader != null)
                    {
                        existingHeader.Amount = newData.Amount;
                        existingHeader.TotalQty = newData.TotalQty;
                        existingHeader.IsCheckout = newData.IsCheckout;
                        existingHeader.UpdateBy = newData.UpdateBy;
                        existingHeader.UpdateDate = DateTime.Now;

                        db.Update(existingHeader);
                        db.SaveChanges();

                        if (newData.OrderDetails != null)
                        {
                            foreach (VMTblTOrderDetail item in newData.OrderDetails)
                            {
                                existingdetail = db.TblTOrderDetails.Find(item.Id);
                                if (existingdetail != null)
                                {
                                    existingProduct = db.TblMProducts.Find(item.ProductId);
                                    existingProduct!.Stock += (existingdetail.Qty - item.Qty);
                                    existingProduct.UpdateDate = newData.UpdateDate;
                                    existingProduct.UpdateBy = existingHeader.UpdateBy;
                                    if (existingProduct.Stock < 0)
                                    {
                                        response.statusCode = HttpStatusCode.BadRequest;    
                                        throw new Exception($"Product {item.ProductName} does not have enough Stoct!");
                                    }
                                    
                                        //update product
                                    db.Update(existingProduct);
                                    db.SaveChanges();
                                    
                                    existingdetail.ProductId = item.ProductId;
                                    existingdetail.Qty = item.Qty;
                                    existingdetail.Price = item.Price;
                                    existingdetail.UpdateBy = item.UpdateBy;
                                    existingdetail.UpdateDate = existingHeader.UpdateDate;

                                    db.Update(existingdetail);
                                    db.SaveChanges();



                                }
                            }
                        }
                    }
                    else
                    {
                        response.statusCode = HttpStatusCode.BadRequest;
                        throw new Exception($"Order with ID = {newData.Id} does not exist!");
                    }
                    dbTrans.Commit();
                    response.data = newData;
                    response.statusCode = HttpStatusCode.OK;
                    response.message = $"{HttpStatusCode.OK} = new order has been Updated";
                }
                catch (Exception ex)
                {
                    dbTrans.Rollback();
                    response.message = $"{HttpStatusCode.InternalServerError}" + ex.Message;
                }
            }
            return response;
        }
    }
}
