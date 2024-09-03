using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace XPOS340.DataModel
{
    [Table("Tbl_M_Product")]
    public partial class TblMProduct
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }
        [Column("name")]
        [StringLength(100)]
        [Unicode(false)]
        public string Name { get; set; } = null!;
        [Column("price", TypeName = "decimal(18, 0)")]
        public decimal? Price { get; set; }
        [Column("stock")]
        public int? Stock { get; set; }
        [Column("variant_id")]
        public int VariantId { get; set; }
        [Column("image")]
        [Unicode(false)]
        public string? Image { get; set; }
        [Column("is_deleted")]
        public bool IsDeleted { get; set; }
        [Column("create_by")]
        public int? CreateBy { get; set; }
        [Column("create_date", TypeName = "datetime")]
        public DateTime? CreateDate { get; set; }
        [Column("update_by")]
        public int? UpdateBy { get; set; }
        [Column("update_date", TypeName = "datetime")]
        public DateTime? UpdateDate { get; set; }
    }
}
