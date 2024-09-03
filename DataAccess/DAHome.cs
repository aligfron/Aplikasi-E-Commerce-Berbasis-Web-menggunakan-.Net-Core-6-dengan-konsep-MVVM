using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XPOS240.ViewModel;
using XPOS340.DataModel;

namespace DataAccess
{
    public class DAHome

    {
        List<VMTblCoba> data = new List<VMTblCoba>();
        private readonly XPOS340Context db;
        
        public DAHome()
        {

            data.Add(
                new VMTblCoba
                {
                    Id = 1,
                    Nama = "test",
                    Description = "test"

                });
            data.Add(
                new VMTblCoba
                {
                    Id = 1,
                    Nama = "percobaan",
                    Description = "test"

                });
        }
        public DAHome(XPOS340Context _db)
        {
            db = _db;
        }
        public List<VMTblCoba> GetAll() =>
            GetByFilter("");

        public VMTblCoba? GetById(int id)
        {
            return (
                from d in db.TblCobas
                where d.IsDeleted == false
                && d.Id == id
                select new VMTblCoba
                {
                    Id = d.Id,
                    Nama = d.Nama,
                    Description = d.Description,
                    IsDeleted = d.IsDeleted,
                    CreateDate = d.CreateDate,
                    UpdateDate = d.UpdateDate
                }
                ).FirstOrDefault();
        }

        public List<VMTblCoba> GetByFilter(string filter)
        {

            return (
                from d in db.TblCobas
                where
                d.IsDeleted == false &&
                (d.Nama.Contains(filter) || d.Description.Contains(filter))
                select new VMTblCoba
                {
                    Id = d.Id,
                    Nama = d.Nama,
                    Description = d.Description,
                    IsDeleted = d.IsDeleted,
                    CreateDate = d.CreateDate,
                    UpdateDate = d.UpdateDate
                }
                ).ToList();
        }
        public List<VMTblCoba>? create(VMTblCoba input)
        {
            try
            {
                TblCoba data = new TblCoba();
                data.Nama = input.Nama;
                data.Description = input.Description;
                
                db.Add(data);

                db.SaveChanges();
                return GetByFilter("");
            }
            catch (Exception e)
            {
                return new List<VMTblCoba>();
            }
            
        }
        public VMTblCoba? Update(VMTblCoba input)
        {
            VMTblCoba? existingdata = GetById(input.Id);

            if (existingdata != null)
            {
                TblCoba newData = new TblCoba()
                {
                    Nama = input.Nama,
                    Description = input.Description,
                    UpdateDate = DateTime.Now,
                    Id = existingdata.Id,
                    IsDeleted = existingdata.IsDeleted,
                    CreateDate = existingdata.CreateDate
                };
                db.Update(newData);
                db.SaveChanges();
            }
            return GetById(input.Id);
        }
        public List<VMTblCoba> Delete(int id)
        {
            VMTblCoba? existingdata = GetById(id);
            //VMTblCoba? newData = data.Where(d => d.id == id).FirstOrDefault();

            if (existingdata != null)
            {
                TblCoba newData = new TblCoba()
                {
                    IsDeleted = true,
                    UpdateDate = DateTime.Now,

                    Nama = existingdata.Nama,
                    Description = existingdata.Description,
                    Id = existingdata.Id,
                    CreateDate = existingdata.CreateDate
                };
                db.Update(newData);
                db.SaveChanges();
            }
            return GetByFilter("");
        }
    }
}
