using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XPOS340.DataModel;

namespace XPOS240.ViewModel
{
    public partial class VMTblMCustomer
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string Address { get; set; } = null!;
        public string? Phone { get; set; }
        public int? RoleId { get; set; }
        public bool IsDeleted { get; set; }
        public int CreateBy { get; set; }
        public DateTime CreateDate { get; set; }
        public int? UpdateBy { get; set; }
        public DateTime? UpdateDate { get; set; }

        public VMTblMCustomer(TblMCustomer customer)
        {
            Id = customer.Id;
            Name = customer.Name;
            Email = customer.Email;
            Password = customer.Password;
            Address = customer.Address;
            Phone = customer.Phone;
            RoleId = customer.RoleId;
            IsDeleted = customer.IsDeleted;
            CreateBy = customer.CreateBy;
            CreateDate = customer.CreateDate;
            UpdateBy = customer.UpdateBy;
            UpdateDate = customer.UpdateDate;

        }
    }
}
