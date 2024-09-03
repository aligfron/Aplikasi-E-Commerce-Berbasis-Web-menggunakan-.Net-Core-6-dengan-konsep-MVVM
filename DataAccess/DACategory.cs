using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
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
        public List<VMTblMCategory> GetByFilter(string filter)
        {
            try
            {
                //throw new Exception("eror");
                return (
                    from c in db.TblMCategories
                    where c.IsDeleted == false
                    && (c.CategoryName.Contains(filter) ||
                    c.Description.Contains(filter))
                    select new VMTblMCategory(c)
                    ).ToList();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }
        public VMTblMCategory? GetById(int id)
        {
            try
            {
                if(id > 0)
                {
                    return (
                        from c in db.TblMCategories
                        where c.IsDeleted == false
                        && (c.Id == id )
                        select new VMTblMCategory(c)
                    ).FirstOrDefault();
                }
                else
                {
                    throw new HttpRequestException("Category ID is not provided");
                }
            }catch(Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public VMTblMCategory Create(VMTblMCategory data)
        {
            
         using (IDbContextTransaction dbTrans = db.Database.BeginTransaction())
         {
            try
                {
                    TblMCategory newData = new TblMCategory();
                    newData.CategoryName = data.CategoryName;
                    newData.Description = data.Description;
                    newData.CreateBy = data.CreateBy;


                    db.Add(newData);
                    db.SaveChanges();
                    dbTrans.Commit();
                    return new VMTblMCategory(newData);
                }
            catch (Exception ex)
            {
                    dbTrans.Rollback();
                throw new Exception(ex.Message);
            }
        }
            
        }

        public VMTblMCategory Update(VMTblMCategory data)
        {
            using (IDbContextTransaction dbTrans = db.Database.BeginTransaction())
            {
                try
                {
                    var existingData = db.TblMCategories
                                         .FirstOrDefault(c => c.Id == data.Id && !c.IsDeleted);

                    if (existingData == null)
                    {
                        throw new Exception("Kategori tidak ditemukan.");
                    }

                    existingData.CategoryName = data.CategoryName;
                    existingData.Description = data.Description;
                    existingData.UpdateBy = data.UpdateBy;
                    existingData.UpdateDate = DateTime.Now;

                    db.Update(existingData);
                    db.SaveChanges();
                    dbTrans.Commit();
                    return new VMTblMCategory(existingData);
                }
                catch (Exception ex)
                {
                    dbTrans.Rollback();
                    throw new Exception("Terjadi kesalahan saat memperbarui kategori.", ex);
                }
            }
        }

        public VMTblMCategory Delete(int id, int userId)
        {
            using (IDbContextTransaction dbTrans = db.Database.BeginTransaction())
            {
                try
                {
                    TblMCategory? existingData = db.TblMCategories
                                                   .FirstOrDefault(c => c.Id == id && !c.IsDeleted);
                    if (existingData == null)
                    {
                        throw new Exception("Kategori tidak ditemukan.");
                    }

                    // Update the necessary fields before marking as deleted
                    existingData.IsDeleted = true;
                    existingData.UpdateBy = userId;
                    existingData.UpdateDate = DateTime.Now;

                    db.Update(existingData);
                    db.SaveChanges();
                    dbTrans.Commit();

                    return new VMTblMCategory(existingData);
                }
                catch (Exception ex)
                {
                    // Rollback the transaction if an error occurs
                    dbTrans.Rollback();
                    throw new Exception("Terjadi kesalahan saat menghapus kategori.", ex);
                }
            }
        }


    }
}
