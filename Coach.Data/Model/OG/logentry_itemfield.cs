//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Coach.Data.Model.OG
{
    using System;
    using System.Collections.Generic;
    
    public partial class logentry_itemfield
    {
        public int idLogEntry_ItemField { get; set; }
        public int idLogEntry { get; set; }
        public int idLogItem_Field { get; set; }
        public Nullable<double> decimalValue { get; set; }
        public string stringValue { get; set; }
        public Nullable<System.DateTime> datetimeValue { get; set; }
    
        public virtual logentry logentry { get; set; }
        public virtual logitem_field logitem_field { get; set; }
    }
}
