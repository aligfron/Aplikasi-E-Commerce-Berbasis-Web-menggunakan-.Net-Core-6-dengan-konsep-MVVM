using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XPOS340.DataModel;

namespace XPOS240.ViewModel
{
    public partial class VMTblTOrder
    {
        public int Id { get; set; }
        
        public string? TrxCode { get; set; }
        
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


        public List<VMTblTOrderDetail>? OrderDetails { get; set; }

        public VMTblTOrder() { }

        public VMTblTOrder(TblTOrderHeader header, TblTOrderDetail detail, TblMCustomer customer)
        {

        }
    }
}
