//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace WEBREPORTMOBILEAPP.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class tbl_Price
    {
        public string Price_ID { get; set; }
        public Nullable<double> sku_barcode { get; set; }
        public string Shop_GroupPrice { get; set; }
        public Nullable<int> Price_Value { get; set; }
        public Nullable<bool> Price_Status { get; set; }
        public Nullable<System.DateTime> Price_DateUpdate { get; set; }
        public Nullable<System.DateTime> Price_SyncFlag { get; set; }
        public Nullable<System.DateTime> Price_DeleteFlag { get; set; }
    }
}
