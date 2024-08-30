using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XPOS340.DataModel
{
    [Table("Tbl_Coba")]
    public class DMTblCoba
    {
        [Key]
        public int id {  get; set; }
        [StringLength(100)]
        public string nama { get; set; } = null!;
        public string? description {  get; set; }

        public DateTime createDate { get; set; }
        public DateTime updateDate { get; set; }
    }
}
