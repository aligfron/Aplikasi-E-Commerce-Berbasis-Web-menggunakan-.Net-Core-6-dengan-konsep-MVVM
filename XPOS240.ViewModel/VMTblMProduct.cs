using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XPOS340.DataModel;

namespace XPOS240.ViewModel
{
    public partial class VMTblMProduct
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public decimal? Price { get; set; }
        public int? Stock { get; set; }
        public int VariantId { get; set; }

        public string? VariantName { get; set; }
        public int? CategoryId { get; set; }

        public string? CategoryName { get; set; }
        public string? Image { get; set; }
        public bool IsDeleted { get; set; }
        public int? CreateBy { get; set; }
        public DateTime? CreateDate { get; set; }
        public int? UpdateBy { get; set; }
        public DateTime? UpdateDate { get; set; }

        public VMTblMProduct() { }
        public VMTblMProduct(TblMProduct product) {
            Id = product.Id;
            Name = product.Name;
            Price = product.Price;
            Stock = product.Stock;
            VariantId = product.VariantId;
            Image = product.Image;
            IsDeleted = product.IsDeleted;
            CreateBy = product.CreateBy;
            CreateDate = product.CreateDate;
            UpdateBy = product.UpdateBy;
            UpdateDate = product.UpdateDate;

        }
        public VMTblMProduct(TblMProduct product, TblMVariant variant, TblMCategory category)
        {
            Id = product.Id;
            Name = product.Name;
            Price = product.Price;
            Stock = product.Stock;
            VariantId = product.VariantId;
            VariantName = variant.Name;
            CategoryId = category.Id;
            CategoryName = category.CategoryName;
            Image = product.Image;
            IsDeleted = product.IsDeleted;
            CreateBy = product.CreateBy;
            CreateDate = product.CreateDate;
            UpdateBy = product.UpdateBy;
            UpdateDate = product.UpdateDate;
        }
    }
}
