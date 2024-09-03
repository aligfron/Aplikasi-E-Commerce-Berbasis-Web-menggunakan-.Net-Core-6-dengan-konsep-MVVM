using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace XPOS340.DataModel
{
    [Table("Tbl_M_Customer")]
    public partial class TblMCustomer
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }
        [Column("name")]
        [StringLength(50)]
        [Unicode(false)]
        public string Name { get; set; } = null!;
        [Column("email")]
        [StringLength(50)]
        [Unicode(false)]
        public string Email { get; set; } = null!;
        [Column("password")]
        [StringLength(100)]
        [Unicode(false)]
        public string Password { get; set; } = null!;
        [Column("address")]
        [Unicode(false)]
        public string Address { get; set; } = null!;
        [Column("phone")]
        [StringLength(15)]
        [Unicode(false)]
        public string? Phone { get; set; }
        [Column("role_id")]
        public int? RoleId { get; set; }
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
