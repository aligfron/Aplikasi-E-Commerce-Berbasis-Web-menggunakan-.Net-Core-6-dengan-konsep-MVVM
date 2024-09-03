using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XPOS240.ViewModel
{
    public partial class VMTblTOrderHeader
    {
        public int Id { get; set; }
        
        public string TrxCode { get; set; } = null!;
        
        public int CustomerId { get; set; }
        public string? CustomerName { get; set; }

        public decimal Amount { get; set; }
        
        public int TotalQty { get; set; }
        
        public bool IsCheckout { get; set; }
        
        public bool IsDeleted { get; set; }
        
        public int CreateBy { get; set; }
        
        public DateTime CreateDate { get; set; }
        
        public int? UpdateBy { get; set; }

        public DateTime? UpdateDate { get; set; }
    }
}
