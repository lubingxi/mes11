//------------------------------------------------------------------------------
// <auto-generated>
//     此代码已从模板生成。
//
//     手动更改此文件可能导致应用程序出现意外的行为。
//     如果重新生成代码，将覆盖对此文件的手动更改。
// </auto-generated>
//------------------------------------------------------------------------------

namespace YLMES.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class FI_Affairs
    {
        public int ID { get; set; }
        public Nullable<int> POID { get; set; }
        public Nullable<int> MaterialID { get; set; }
        public Nullable<int> PrintQTY { get; set; }
        public Nullable<int> TypeID { get; set; }
        public Nullable<double> Price { get; set; }
        public Nullable<double> Amount { get; set; }
        public string CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedTime { get; set; }
        public string QA { get; set; }
        public string Purchaser { get; set; }
        public string Sender { get; set; }
        public string Status { get; set; }
        public string Remark { get; set; }
    }
}