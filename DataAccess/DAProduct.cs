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
    public class DAProduct
    {
        private readonly XPOS340Context db;
        public DAProduct(XPOS340Context _db) {
            db = _db;
        }


        public VMResponse<List<VMTblMProduct>> GetByFilter(string filter)
        {
            VMResponse<List<VMTblMProduct>> response = new VMResponse<List<VMTblMProduct>>();
            try
            {
                decimal parsedPrice = 0;
                int parsedStock = 0;
                bool isPriceParsed = decimal.TryParse(filter, out parsedPrice);
                bool isStockParsed = int.TryParse(filter, out parsedStock);

                response.data = (
                    from c in db.TblMProducts
                    join v in db.TblMVariants on c.VariantId equals v.Id
                    join k in db.TblMCategories on v.CategoryId equals k.Id
                    where c.IsDeleted == false
                       && (c.Name.Contains(filter)
                           || (isPriceParsed && c.Price == parsedPrice)
                           || (isStockParsed && c.Stock == parsedStock)
                           || v.Name.Contains(filter)
                           || (c.Image!.Contains(filter))
                           || k.CategoryName.Contains(filter))
                    select new VMTblMProduct(c, v, k)
                ).ToList();

                response.message = (response.data.Count > 0)
                    ? $"{response.data.Count} of Product(s) found successfully."
                    : $"{HttpStatusCode.NoContent} - No data found";

                response.statusCode = (response.data.Count > 0)
                    ? HttpStatusCode.OK
                    : HttpStatusCode.NoContent;
            }
            catch (Exception ex)
            {
                response.message = $"{HttpStatusCode.InternalServerError} - {ex.Message}";
                response.statusCode = HttpStatusCode.InternalServerError;
            }
            return response;
        }

        public VMResponse<VMTblMProduct?> GetById(int id)
        {
            VMResponse<VMTblMProduct?> response = new VMResponse<VMTblMProduct?>();
            try
            {
                if (id > 0)
                {
                    response.data = (

                         from c in db.TblMProducts
                         join v in db.TblMVariants on c.VariantId equals v.Id
                         join k in db.TblMCategories on v.CategoryId equals k.Id
                         where c.IsDeleted == false && c.Id == id
                         select new VMTblMProduct(c, v, k)
                    ).FirstOrDefault();

                    if (response.data != null)
                    {
                        response.statusCode = HttpStatusCode.OK;
                        response.message = $"{HttpStatusCode.OK} - Product Sukses Full";
                    }
                    else
                    {
                        response.statusCode = HttpStatusCode.NoContent;
                        response.message = $"{HttpStatusCode.NoContent} - Product does not exis";
                    }
                }
                else
                {
                    response.statusCode = HttpStatusCode.BadRequest;
                    response.message = $"{HttpStatusCode.BadRequest} - please input Product";
                }
            }
            catch (Exception e)
            {

                response.message = $"{HttpStatusCode.InternalServerError} - {e.Message}";
            }
            return response;
        }

        public VMResponse<VMTblMProduct?> Create(VMTblMProduct data)
        {
            var response = new VMResponse<VMTblMProduct?>();
            using (IDbContextTransaction dbTrans = db.Database.BeginTransaction())
            {
                try
                {
                    TblMProduct newData = new TblMProduct
                    {
                        Name = data.Name,
                        Price = data.Price,
                        Stock = data.Stock,
                        VariantId = data.VariantId,
                        Image = data.Image,
                        CreateBy = data.CreateBy,
                        CreateDate = DateTime.Now, 
                        IsDeleted = false 
                    };

                   
                    db.Add(newData);
                    db.SaveChanges();
                    dbTrans.Commit();

                    
                    response.data = new VMTblMProduct(newData);
                    response.statusCode = HttpStatusCode.Created;
                    response.message = $"{HttpStatusCode.Created} - New Product successfully created.";
                }
                catch (Exception ex)
                {
                    dbTrans.Rollback();

                    
                    response.data = null;
                    response.statusCode = HttpStatusCode.InternalServerError;
                    response.message = $"{HttpStatusCode.InternalServerError} - {ex.Message}";
                }
            }

            return response;
        }


        public VMResponse<VMTblMProduct?> Update(VMTblMProduct data)
        {
            var response = new VMResponse<VMTblMProduct?>();
            using (IDbContextTransaction dbTrans = db.Database.BeginTransaction())
            {
                try
                {
                    
                    var existingData = db.TblMProducts
                                         .FirstOrDefault(c => c.Id == data.Id && !c.IsDeleted);

                    if (existingData == null)
                    {
                        response.statusCode = HttpStatusCode.NotFound;
                        response.message = $"{HttpStatusCode.NotFound} - Product Not Found";
                        return response; 
                    }

                    
                    existingData.Name = data.Name;
                    existingData.Price = data.Price;
                    existingData.Stock = data.Stock;
                    existingData.VariantId = data.VariantId;
                    existingData.Image = data.Image;
                    existingData.UpdateBy = data.UpdateBy;
                    existingData.UpdateDate = DateTime.Now;

                    db.Update(existingData);
                    db.SaveChanges();
                    dbTrans.Commit();

                    
                    response.data = new VMTblMProduct(existingData);
                    response.statusCode = HttpStatusCode.OK;
                    response.message = $"{HttpStatusCode.OK} - Product Has Been Updated";
                }
                catch (Exception ex)
                {
                    dbTrans.Rollback();
                    response.statusCode = HttpStatusCode.InternalServerError;
                    response.message = $"{HttpStatusCode.InternalServerError} - {ex.Message}";
                }
            }
            return response;
        }


        public VMResponse<VMTblMProduct> Delete(int id, int userId)

        {
            VMResponse<VMTblMProduct?> response = new VMResponse<VMTblMProduct?>();
            using (IDbContextTransaction dbTrans = db.Database.BeginTransaction())
            {
                try
                {
                    TblMProduct? existingData = db.TblMProducts
                                                   .FirstOrDefault(c => c.Id == id && !c.IsDeleted);
                    if (existingData == null)
                    {

                        response.statusCode = HttpStatusCode.NotFound;
                        response.message = $"{HttpStatusCode.NotFound} - Product Not Fount";
                    }

                    existingData.Image = null;
                    existingData.IsDeleted = true;
                    existingData.UpdateBy = userId;
                    existingData.UpdateDate = DateTime.Now;

                    db.Update(existingData);
                    db.SaveChanges();
                    dbTrans.Commit();

                    response.data = new VMTblMProduct(existingData);

                    response.statusCode = HttpStatusCode.OK;
                    response.message = $"{HttpStatusCode.OK} - Category Has been Deleted";
                }
                catch (Exception ex)
                {
                    
                    dbTrans.Rollback();
                    response.message = $"{HttpStatusCode.InternalServerError} - {ex.Message}";
                }
            }
            return response;
        }
    }
}
