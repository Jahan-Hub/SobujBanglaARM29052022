//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace SobujBanglaARM
{
    using System;
    using System.Collections.Generic;
    
    public partial class tblPurchaseHDR
    {
        public string PurID { get; set; }
        public string BillNo { get; set; }
        public Nullable<System.DateTime> BillDate { get; set; }
        public int SupplierId { get; set; }
        public string Remarks { get; set; }
        public Nullable<decimal> PacketCost { get; set; }
        public Nullable<decimal> LabourCost { get; set; }
        public Nullable<decimal> OtherCost { get; set; }
        public Nullable<decimal> Discount { get; set; }
        public Nullable<decimal> NetAmount { get; set; }
        public Nullable<decimal> Paid { get; set; }
        public Nullable<decimal> Due { get; set; }
        public string PayMode { get; set; }
        public string ChequeNo { get; set; }
        public Nullable<System.DateTime> ChequeDt { get; set; }
        public string BankName { get; set; }
        public string ChequeDetails { get; set; }
        public string UserId { get; set; }
        public Nullable<System.DateTime> Lmdt { get; set; }
        public Nullable<decimal> Balance { get; set; }
        public string OpBalance { get; set; }
        public Nullable<int> IdClient { get; set; }
        public Nullable<decimal> AgentCost { get; set; }
    }
}
