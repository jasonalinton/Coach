//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Coach.ServiceOG._Data._CoachModel
{
    using System;
    using System.Collections.Generic;
    
    public partial class inventoryitem_todoitem
    {
        public int idInventoryItem_TodoItem { get; set; }
        public int idInventoryItem { get; set; }
        public int idToDoItem { get; set; }
        public Nullable<bool> isVisible { get; set; }
        public System.DateTime createdAt { get; set; }
        public System.DateTime updatedAt { get; set; }
    
        public virtual inventoryitem inventoryitem { get; set; }
        public virtual todoitem todoitem { get; set; }
    }
}
