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
    public class DAVariant
    {
        private readonly XPOS340Context db;
        private DACategory kategori;
        public DAVariant(XPOS340Context _db) {
            db = _db;
           kategori = new DACategory( _db);
        }
        public VMResponse<List<VMTblMVariant>> GetByFilter(string filter)
        {
            VMResponse<List<VMTblMVariant>> response = new VMResponse<List<VMTblMVariant>>();
            try
            {
                response.data = (
                    from v in db.TblMVariants
                    join c in db.TblMCategories on v.CategoryId equals c.Id
                    where v.IsDeleted == false && (c.CategoryName.Contains(filter) || v.Name.Contains(filter) || v.Description!.Contains(filter))
                    select new VMTblMVariant(v, c)
                    ).ToList();
                if (response.data != null && response.data.Count > 0)
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
            catch (Exception ex)
            {
                response.message = $"{HttpStatusCode.InternalServerError} - {ex.Message}";
            }
            return response;
        }
        public VMResponse<List<VMTblMVariant>> GetByCategory(int categoryId)
        {
            VMResponse<List<VMTblMVariant>> response = new VMResponse<List<VMTblMVariant>>();
            try
            {
                response.data = (
                    from v in db.TblMVariants
                    join c in db.TblMCategories on v.CategoryId equals c.Id
                    where v.IsDeleted == false && v.CategoryId == categoryId
                    select new VMTblMVariant(v, c)
                    ).ToList();
                if (response.data != null && response.data.Count > 0)
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
            catch (Exception ex)
            {
                response.message = $"{HttpStatusCode.InternalServerError} - {ex.Message}";
            }
            return response;
        }
        public VMResponse<VMTblMVariant?> GetById(int id)
        {
            VMResponse<VMTblMVariant?> response = new VMResponse<VMTblMVariant?>();
            try
            {
                if (id > 0)
                {
                    response.data = (
                    from v in db.TblMVariants
                    join c in db.TblMCategories on v.CategoryId equals c.Id
                    where v.IsDeleted == false && v.Id == id
                    select new VMTblMVariant(v,c)
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

        public VMResponse<VMTblMVariant?> Create(VMTblMVariant data)
        {
            var response = new VMResponse<VMTblMVariant?>();
            using (IDbContextTransaction dbTrans = db.Database.BeginTransaction())
            {
                try
                {
                    TblMVariant newData = new TblMVariant
                        {

                            CategoryId = data.CategoryId,
                            Name = data.Name,
                            Description = data.Description,
                            CreateBy = data.CreateBy,
                            CreateDate = DateTime.Now, 
                            IsDeleted = false 
                        };


                    db.Add(newData);
                    db.SaveChanges();
                    dbTrans.Commit();


                    response.data = new VMTblMVariant(newData);
                    response.statusCode = HttpStatusCode.Created;
                    response.message = $"{HttpStatusCode.Created} - New Variant successfully created.";
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
        public VMResponse<VMTblMVariant> update(VMTblMVariant data)
        {
            var response = new VMResponse<VMTblMVariant>();
            using (IDbContextTransaction dbTrans = db.Database.BeginTransaction())
                try
                {
                    var existingData = db.TblMVariants
                                         .FirstOrDefault(c => c.Id == data.Id && !c.IsDeleted);

                    if (existingData == null)
                    {
                        response.statusCode = HttpStatusCode.NotFound;
                        response.message = $"{HttpStatusCode.NotFound} - Category Not Found";
                        return response; // Exit early if the category doesn't exist
                    }

                    // Update the existing data with the new data
                    existingData.CategoryId = data.CategoryId;
                    existingData.Name = data.Name;
                    existingData.Description = data.Description;
                    existingData.UpdateBy = data.UpdateBy;
                    existingData.UpdateDate = DateTime.Now;

                    db.Update(existingData);
                    db.SaveChanges();
                    dbTrans.Commit();

                    // Set the response with the updated data and a success message
                    response.data = new VMTblMVariant(existingData);
                    response.statusCode = HttpStatusCode.OK;
                    response.message = $"{HttpStatusCode.OK} - Category {response.data.CategoryName} Has Been Updated";
                }
                catch (Exception ex)
                {
                    // Rollback the transaction in case of an error
                    dbTrans.Rollback();
                    response.statusCode = HttpStatusCode.InternalServerError;
                    response.message = $"{HttpStatusCode.InternalServerError} - {ex.Message}";
                }
            return response;
        }
        public VMResponse<VMTblMVariant> delete(int id, int userid)
        {
            VMResponse<VMTblMVariant?> response = new VMResponse<VMTblMVariant?>();
            using (IDbContextTransaction dbTrans = db.Database.BeginTransaction())
            {
                try
                {
                    TblMVariant? existingData = db.TblMVariants
                                                   .FirstOrDefault(c => c.Id == id && !c.IsDeleted);
                    if (existingData == null)
                    {

                        response.statusCode = HttpStatusCode.NotFound;
                        response.message = $"{HttpStatusCode.NotFound} - Category Not Fount";
                    }

                    // Update the necessary fields before marking as deleted
                    existingData.IsDeleted = true;
                    existingData.UpdateBy = userid;
                    existingData.UpdateDate = DateTime.Now;

                    db.Update(existingData);
                    db.SaveChanges();
                    dbTrans.Commit();

                    response.data = new VMTblMVariant(existingData);

                    response.statusCode = HttpStatusCode.OK;
                    response.message = $"{HttpStatusCode.OK} -  Has been Deleted";
                }
                catch (Exception ex)
                {
                    // Rollback the transaction if an error occurs
                    dbTrans.Rollback();
                    response.message = $"{HttpStatusCode.InternalServerError} - {ex.Message}";
                }
            }
            return response;
        }
    }
}
