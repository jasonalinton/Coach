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
    
    public partial class logitemfield_keyvalue
    {
        public int idLogItemField_KeyValue { get; set; }
        public int idLogItemField { get; set; }
        public int idKeyValue { get; set; }
        public Nullable<bool> isActive { get; set; }
        public Nullable<System.DateTime> createdAt { get; set; }
        public Nullable<System.DateTime> updatedAt { get; set; }
    
        public virtual keyvalue keyvalue { get; set; }
        public virtual logitemfield logitemfield { get; set; }
    }
}