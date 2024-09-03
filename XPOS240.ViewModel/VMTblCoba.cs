using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XPOS240.ViewModel
{
    public class VMTblCoba
    {
        
        public int id { get; set; }
        
        public string nama { get; set; } = null!;
        public string? description { get; set; }
        public bool isDeleted { get; set; }
        public DateTime createDate { get; set; }
        public DateTime? updateDate { get; set; }
    }
}
