//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Dmart.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class tbl_invoiceDetail
    {
        public int o_id { get; set; }
        public Nullable<int> o_fk_pro_id { get; set; }
        public Nullable<int> total_bill { get; set; }
        public Nullable<System.DateTime> order_date { get; set; }
        public Nullable<int> qty { get; set; }
        public Nullable<int> price { get; set; }
        public Nullable<int> inv_fk_id { get; set; }
    
        public virtual tbl_invoice tbl_invoice { get; set; }
        public virtual tbl_product tbl_product { get; set; }
    }
}