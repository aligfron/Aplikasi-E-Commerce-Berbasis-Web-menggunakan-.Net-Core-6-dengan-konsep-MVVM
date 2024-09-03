using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace XPOS340.DataModel
{
    [Table("Tbl_T_Order_Detail")]
    public partial class TblTOrderDetail
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }
        [Column("order_header_id")]
        public int OrderHeaderId { get; set; }
        [Column("product_id")]
        public int ProductId { get; set; }
        [Column("qty")]
        public int Qty { get; set; }
        [Column("price", TypeName = "decimal(18, 2)")]
        public decimal Price { get; set; }
        [Column("is_deleted")]
        public bool IsDeleted { get; set; }
        [Column("create_by")]
        public int CreateBy { get; set; }
        [Column("create_date", TypeName = "datetime")]
        public DateTime CreateDate { get; set; }
        [Column("update_by")]
        public int? UpdateBy { get; set; }
        [Column("update_date", TypeName = "datetime")]
        public DateTime? UpdateDate { get; set; }
    }
}
