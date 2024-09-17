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
    public class DACustomer
    {
        private readonly XPOS340Context db;
        public DACustomer(XPOS340Context _db)
        {
            db = _db;
        }

        public VMResponse<List<VMTblMCustomer>> GetByFilter(string filter)
        {
            VMResponse<List<VMTblMCustomer>> response = new VMResponse<List<VMTblMCustomer>>();
            try
            {
                response.data = (
                    from a in db.TblMCustomers
                    where a.IsDeleted == false
                    && (a.Name.Contains(filter) || a.Email.Contains(filter) || a.Address.Contains(filter) || a.Phone!.Contains(filter) || a.Password.Contains(filter)
                    )select new VMTblMCustomer(a) ).ToList();
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

        public VMResponse<VMTblMCustomer?> GetById(int id)
        {
            VMResponse<VMTblMCustomer?> response = new VMResponse<VMTblMCustomer?>();
            try
            {
                if (id > 0)
                {
                    response.data = (

                        from c in db.TblMCustomers
                        where c.IsDeleted == false
                        && (c.Id == id)
                        select new VMTblMCustomer(c)
                    ).FirstOrDefault();

                    if (response.data != null)
                    {
                        response.statusCode = HttpStatusCode.OK;
                        response.message = $"{HttpStatusCode.OK} - Category Sukses Full";
                    }
                    else
                    {
                        response.statusCode = HttpStatusCode.NoContent;
                        response.message = $"{HttpStatusCode.NoContent} - Category does not exis";
                    }
                }
                else
                {
                    response.statusCode = HttpStatusCode.BadRequest;
                    response.message = $"{HttpStatusCode.BadRequest} - please input category";
                }
            }
            catch (Exception e)
            {

                response.message = $"{HttpStatusCode.InternalServerError} - {e.Message}";
            }
            return response;
        }

        public VMResponse<VMTblMCustomer?> GetByEmail(string email)
        {
            VMResponse<VMTblMCustomer?> response = new VMResponse<VMTblMCustomer?>();
            try
            {
               response.data = (

                        from c in db.TblMCustomers
                        where c.IsDeleted == false
                        && (c.Email == email)
                        select new VMTblMCustomer(c)
                    ).FirstOrDefault();

                    if (response.data != null)
                    {
                        response.statusCode = HttpStatusCode.OK;
                        response.message = $"{HttpStatusCode.OK} - Category Sukses Full";
                    }
                    else
                    {
                        response.statusCode = HttpStatusCode.NoContent;
                        response.message = $"{HttpStatusCode.NoContent} - Category does not exis";
                    }
                
              
            }
            catch (Exception e)
            {

                response.message = $"{HttpStatusCode.InternalServerError} - {e.Message}";
            }
            return response;
        }
        public VMResponse<VMTblMCustomer?> Create(VMTblMCustomer data)
        {
            var response = new VMResponse<VMTblMCustomer?>();
            using (IDbContextTransaction dbTrans = db.Database.BeginTransaction())
            {
                try
                {
                    TblMCustomer newData = new TblMCustomer
                    {
                        Name = data.Name,
                        Email = data.Email,
                        Password = data.Password,
                        Address = data.Address,
                        Phone = data.Phone,
                        RoleId = data.RoleId,
                        CreateBy = data.CreateBy,
                        CreateDate = DateTime.Now,
                        IsDeleted = false
                    };


                    db.Add(newData);
                    db.SaveChanges();
                    dbTrans.Commit();


                    response.data = new VMTblMCustomer(newData);
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


        public VMResponse<VMTblMCustomer?> Update(VMTblMCustomer data)
        {
            var response = new VMResponse<VMTblMCustomer?>();
            using (IDbContextTransaction dbTrans = db.Database.BeginTransaction())
            {
                try
                {

                    var existingData = db.TblMCustomers
                                         .FirstOrDefault(c => c.Id == data.Id && !c.IsDeleted);

                    if (existingData == null)
                    {
                        response.statusCode = HttpStatusCode.NotFound;
                        response.message = $"{HttpStatusCode.NotFound} - Product Not Found";
                        return response;
                    }


                    existingData.Name = data.Name;
                    existingData.Email = data.Email;
                    existingData.Password = data.Password;
                    existingData.Address = data.Address;
                    existingData.Phone = data.Phone;
                    existingData.RoleId = data.RoleId;
                    existingData.UpdateBy = data.UpdateBy;
                    existingData.UpdateDate = DateTime.Now;

                    db.Update(existingData);
                    db.SaveChanges();
                    dbTrans.Commit();


                    response.data = new VMTblMCustomer(existingData);
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


        public VMResponse<VMTblMCustomer> Delete(int id, int userId)

        {
            VMResponse<VMTblMCustomer?> response = new VMResponse<VMTblMCustomer?>();
            using (IDbContextTransaction dbTrans = db.Database.BeginTransaction())
            {
                try
                {
                    TblMCustomer? existingData = db.TblMCustomers
                                                   .FirstOrDefault(c => c.Id == id && !c.IsDeleted);
                    if (existingData == null)
                    {

                        response.statusCode = HttpStatusCode.NotFound;
                        response.message = $"{HttpStatusCode.NotFound} - Product Not Fount";
                    }


                    existingData.IsDeleted = true;
                    existingData.UpdateBy = userId;
                    existingData.UpdateDate = DateTime.Now;

                    db.Update(existingData);
                    db.SaveChanges();
                    dbTrans.Commit();

                    response.data = new VMTblMCustomer(existingData);

                    response.statusCode = HttpStatusCode.OK;
                    response.message = $"{HttpStatusCode.OK} - Category  Has been Deleted";
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
