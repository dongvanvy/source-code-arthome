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
    
    public partial class tbl_bill
    {
        public string Bill_id { get; set; }
        public string Us_id { get; set; }
        public string Shop_id { get; set; }
        public Nullable<System.DateTime> Bill_date { get; set; }
        public Nullable<System.DateTime> Bill_deleteFlag { get; set; }
        public Nullable<System.DateTime> Bill_SyncFlag { get; set; }
        public string Bill_SDTKH { get; set; }
        public string Bill_TenKH { get; set; }
        public string Bill_ImageKH { get; set; }
        public string Bill_OTP { get; set; }
        public string Bill_OTPConfirm { get; set; }
    }
}
