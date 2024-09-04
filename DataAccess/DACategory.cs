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
    public class DACategory

    {
        private readonly XPOS340Context db;



        public DACategory(XPOS340Context _db) { 
                db = _db;
        }
        public VMResponse<List<VMTblMCategory>> GetByFilter(string filter)
        {
            VMResponse<List<VMTblMCategory>> response = new VMResponse<List<VMTblMCategory>>();
            try
            {
                //throw new Exception("eror");
                response.data = (
                    from c in db.TblMCategories
                    where c.IsDeleted == false
                    && (c.CategoryName.Contains(filter) ||
                    c.Description.Contains(filter))
                    select new VMTblMCategory(c)
                    ).ToList();
                response.message = (response.data.Count > 0)
                    ? $"{response.data.Count} of category sukses"
                    : $"{HttpStatusCode.NoContent} - no data";

                response.statusCode = (response.data.Count > 0)
                    ? HttpStatusCode.OK
                    : HttpStatusCode.NoContent;
            }
            catch (Exception ex)
            {
                response.message = $"{HttpStatusCode.InternalServerError} - {ex.Message}";
            }
            return response;
        }
        public VMResponse<VMTblMCategory?> GetById(int id)
        {
            VMResponse<VMTblMCategory?> response = new VMResponse<VMTblMCategory?>();
            try
            {
                if(id > 0)
                {
                    response.data = (
                     
                        from c in db.TblMCategories
                        where c.IsDeleted == false
                        && (c.Id == id )
                        select new VMTblMCategory(c)
                    ).FirstOrDefault();

                    if(response.data != null)
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
            }catch(Exception e)
            {
                
                response.message = $"{HttpStatusCode.InternalServerError} - {e.Message}";
            }
            return response;
        }

        public VMResponse<VMTblMCategory?> Create(VMTblMCategory data)
        {
            var response = new VMResponse<VMTblMCategory?>();
            using (IDbContextTransaction dbTrans = db.Database.BeginTransaction())
            {
                try
                {
                    // Create a new TblMCategory object and populate its properties
                    TblMCategory newData = new TblMCategory
                    {
                        CategoryName = data.CategoryName,
                        Description = data.Description,
                        CreateBy = data.CreateBy,
                        CreateDate = DateTime.Now, // Assuming you want to set the current date as the creation date
                        IsDeleted = false // Assuming new entries are not deleted by default
                    };

                    // Add the new entry to the database
                    db.Add(newData);
                    db.SaveChanges();
                    dbTrans.Commit();

                    // Set the response data and message for a successful creation
                    response.data = new VMTblMCategory(newData);
                    response.statusCode = HttpStatusCode.Created;
                    response.message = $"{HttpStatusCode.Created} - New Category successfully created.";
                }
                catch (Exception ex)
                {
                    // Rollback the transaction in case of an error
                    dbTrans.Rollback();

                    // Set the response message and status code for an error
                    response.data = null;
                    response.statusCode = HttpStatusCode.InternalServerError;
                    response.message = $"{HttpStatusCode.InternalServerError} - {ex.Message}";
                }
            }

            return response;
        }


        public VMResponse<VMTblMCategory?> Update(VMTblMCategory data)
        {
            var response = new VMResponse<VMTblMCategory?>();
            using (IDbContextTransaction dbTrans = db.Database.BeginTransaction())
            {
                try
                {
                    // Retrieve the existing category data from the database
                    var existingData = db.TblMCategories
                                         .FirstOrDefault(c => c.Id == data.Id && !c.IsDeleted);

                    if (existingData == null)
                    {
                        response.statusCode = HttpStatusCode.NotFound;
                        response.message = $"{HttpStatusCode.NotFound} - Category Not Found";
                        return response; // Exit early if the category doesn't exist
                    }

                    // Update the existing data with the new data
                    existingData.CategoryName = data.CategoryName;
                    existingData.Description = data.Description;
                    existingData.UpdateBy = data.UpdateBy;
                    existingData.UpdateDate = DateTime.Now;

                    db.Update(existingData);
                    db.SaveChanges();
                    dbTrans.Commit();

                    // Set the response with the updated data and a success message
                    response.data = new VMTblMCategory(existingData);
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
            }
            return response;
        }


        public VMResponse<VMTblMCategory> Delete(int id, int userId)

        {
            VMResponse<VMTblMCategory?> response = new VMResponse<VMTblMCategory?>();
            using (IDbContextTransaction dbTrans = db.Database.BeginTransaction())
            {
                try
                {
                    TblMCategory? existingData = db.TblMCategories
                                                   .FirstOrDefault(c => c.Id == id && !c.IsDeleted);
                    if (existingData == null)
                    {

                        response.statusCode = HttpStatusCode.NotFound;
                        response.message = $"{HttpStatusCode.NotFound} - Category Not Fount";
                    }

                    // Update the necessary fields before marking as deleted
                    existingData.IsDeleted = true;
                    existingData.UpdateBy = userId;
                    existingData.UpdateDate = DateTime.Now;

                    db.Update(existingData);
                    db.SaveChanges();
                    dbTrans.Commit();

                    response.data = new VMTblMCategory(existingData);

                    response.statusCode = HttpStatusCode.OK;
                    response.message = $"{HttpStatusCode.OK} - Category {response.data.CategoryName} Has been Deleted";
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
