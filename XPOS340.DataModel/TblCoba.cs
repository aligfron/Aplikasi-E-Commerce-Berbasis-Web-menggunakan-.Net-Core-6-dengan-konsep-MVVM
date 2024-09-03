using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace XPOS340.DataModel
{
    [Table("Tbl_Coba")]
    public partial class TblCoba
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }
        [Column("nama")]
        [StringLength(100)]
        public string Nama { get; set; } = null!;
        [Column("description")]
        public string? Description { get; set; }
        [Column("createDate")]
        public DateTime CreateDate { get; set; }
        [Column("updateDate")]
        public DateTime? UpdateDate { get; set; }
        [Required]
        [Column("isDeleted")]
        public bool IsDeleted { get; set; }
    }
}
