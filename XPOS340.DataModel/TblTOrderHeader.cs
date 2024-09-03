using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace XPOS340.DataModel
{
    [Table("Tbl_T_Order_Header")]
    public partial class TblTOrderHeader
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }
        [Column("trx_code")]
        [StringLength(100)]
        [Unicode(false)]
        public string TrxCode { get; set; } = null!;
        [Column("customer_id")]
        public int CustomerId { get; set; }
        [Column("amount", TypeName = "decimal(18, 2)")]
        public decimal Amount { get; set; }
        [Column("total_qty")]
        public int TotalQty { get; set; }
        [Column("is_checkout")]
        public bool IsCheckout { get; set; }
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
