using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XPOS240.ViewModel
{

    public partial class VMTblTOrderDetail
    {

        public int Id { get; set; }

        public int OrderHeaderId { get; set; }

        public int ProductId { get; set; }
        public string? ProductName { get; set; } 

        public int Qty { get; set; }

        public decimal Price { get; set; }

        public bool IsDeleted { get; set; }

        public int CreateBy { get; set; }

        public DateTime CreateDate { get; set; }

        public int? UpdateBy { get; set; }

        public DateTime? UpdateDate { get; set; }
    }
}
