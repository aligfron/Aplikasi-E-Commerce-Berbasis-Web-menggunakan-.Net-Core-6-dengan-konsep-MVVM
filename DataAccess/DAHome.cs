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
                    id = 1,
                    nama = "test",
                    description = "test"

                });
            data.Add(
                new VMTblCoba
                {
                    id = 1,
                    nama = "percobaan",
                    description = "test"

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
                where d.isDeleted == false
                && d.id == id
                select new VMTblCoba
                {
                    id = d.id,
                    nama = d.nama,
                    description = d.description,
                    isDeleted = d.isDeleted,
                    createDate = d.createDate,
                    updateDate = d.updateDate
                }
                ).FirstOrDefault();
        }

        public List<VMTblCoba> GetByFilter(string filter)
        {

            return (
                from d in db.TblCobas
                where
                d.isDeleted == false &&
                (d.nama.Contains(filter) || d.description.Contains(filter))
                select new VMTblCoba
                {
                    id = d.id,
                    nama = d.nama,
                    description = d.description,
                    isDeleted = d.isDeleted,
                    createDate = d.createDate,
                    updateDate = d.updateDate
                }
                ).ToList();
        }
        public List<VMTblCoba>? create(VMTblCoba input)
        {
            try
            {
                DMTblCoba data = new DMTblCoba();
                data.nama = input.nama;
                data.description = input.description;
                
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
            VMTblCoba? existingdata = GetById(input.id);

            if (existingdata != null)
            {
                DMTblCoba newData = new DMTblCoba()
                {
                    nama = input.nama,
                    description = input.description,
                    updateDate = DateTime.Now,
                    id = existingdata.id,
                    isDeleted = existingdata.isDeleted,
                    createDate = existingdata.createDate
                };
                db.Update(newData);
                db.SaveChanges();
            }
            return GetById(input.id);
        }
        public List<VMTblCoba> Delete(int id)
        {
            VMTblCoba? existingdata = GetById(id);
            //VMTblCoba? newData = data.Where(d => d.id == id).FirstOrDefault();

            if (existingdata != null)
            {
                DMTblCoba newData = new DMTblCoba()
                {
                    isDeleted = true,
                    updateDate = DateTime.Now,

                    nama = existingdata.nama,
                    description = existingdata.description,
                    id = existingdata.id,
                    createDate = existingdata.createDate
                };
                db.Update(newData);
                db.SaveChanges();
            }
            return GetByFilter("");
        }
    }
}
