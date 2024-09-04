using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XPOS340.DataModel;

namespace XPOS240.ViewModel
{
    public partial class VMTblMVariant
    {
        public int Id { get; set; }
        public int CategoryId { get; set; }

        public string? CategoryName { get; set; }
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public bool IsDeleted { get; set; }
        public int CreateBy { get; set; }
        public DateTime CreateDate { get; set; }
        public int? UpdateBy { get; set; }
        public DateTime? UpdateDate { get; set; }

        public VMTblMVariant() { }

        public VMTblMVariant(TblMVariant variant)
        {
            Id = variant.Id;
            CategoryId = variant.CategoryId;
            Name = variant.Name;
            Description = variant.Description;
            IsDeleted = variant.IsDeleted;
            CreateBy = variant.CreateBy;
            CreateDate = variant.CreateDate;
            UpdateBy = variant.UpdateBy;
            UpdateDate = variant.UpdateDate;
        }
        public VMTblMVariant(TblMVariant variant, TblMCategory category)
        {
            Id = variant.Id;
            CategoryId = variant.CategoryId;
            CategoryName = category.CategoryName;
            Name = variant.Name;
            Description = variant.Description;
            IsDeleted = variant.IsDeleted;
            CreateBy = variant.CreateBy;
            CreateDate = variant.CreateDate;
            UpdateBy = variant.UpdateBy;
            UpdateDate = variant.UpdateDate;
        }
    }
}
