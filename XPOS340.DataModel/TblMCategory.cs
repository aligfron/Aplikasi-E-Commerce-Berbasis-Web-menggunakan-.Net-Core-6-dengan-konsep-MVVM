using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace XPOS340.DataModel
{
    [Table("Tbl_M_Categories")]
    public partial class TblMCategory
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }
        [Column("category_name")]
        [StringLength(100)]
        [Unicode(false)]
        public string CategoryName { get; set; } = null!;
        [Column("description")]
        [StringLength(100)]
        [Unicode(false)]
        public string Description { get; set; } = null!;
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
