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
    
    public partial class inventoryitem_logitem
    {
        public int idInventoryItem_LogItem { get; set; }
        public int idInventoryItem { get; set; }
        public int idLogItem { get; set; }
        public Nullable<int> position { get; set; }
        public bool isActive { get; set; }
        public System.DateTime createdAt { get; set; }
        public System.DateTime updatedAt { get; set; }
    
        public virtual inventoryitem inventoryitem { get; set; }
        public virtual logitem logitem { get; set; }
    }
}
