using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using XPOS340.DataModel;

namespace XPOS240.ViewModel
{
    public partial class VMTblMCategory
    {
        public HttpStatusCode statusCode;

        public int Id { get; set; }
        public string CategoryName { get; set; } = null!;
        public string Description { get; set; } = null!;
        public bool IsDeleted { get; set; }
        public int CreateBy { get; set; }
        public DateTime CreateDate { get; set; }
        public int? UpdateBy { get; set; }
        public DateTime? UpdateDate { get; set; }
        public VMTblMCategory() { }
        public VMTblMCategory(TblMCategory category) {
            Id = category.Id;
            CategoryName = category.CategoryName;
            Description = category.Description;
            IsDeleted = category.IsDeleted;
            CreateBy = category.CreateBy;
            CreateDate = category.CreateDate;
            UpdateBy = category.UpdateBy;
            UpdateDate = category.UpdateDate;
        }
    }
}
