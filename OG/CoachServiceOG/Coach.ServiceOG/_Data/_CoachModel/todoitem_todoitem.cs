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
    
    public partial class todoitem_todoitem
    {
        public int idTodoItem_TodoItem { get; set; }
        public int idTodoItem { get; set; }
        public int idParentTodoItem { get; set; }
        public Nullable<int> levelDown { get; set; }
        public Nullable<int> levelUp { get; set; }
        public System.DateTime createdAt { get; set; }
        public System.DateTime updatedAt { get; set; }
    
        public virtual todoitem todoitem { get; set; }
        public virtual todoitem todoitem1 { get; set; }
    }
}
