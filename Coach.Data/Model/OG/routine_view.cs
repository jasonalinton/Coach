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
    
    public partial class routine_view
    {
        public int idRoutine { get; set; }
        public string title { get; set; }
        public string description { get; set; }
        public string types { get; set; }
        public string inventoryItems { get; set; }
        public string goals { get; set; }
        public string todoItems { get; set; }
        public string inventoryItems_TodoItem { get; set; }
        public string goals_TodoItem { get; set; }
        public string repetitions { get; set; }
        public Nullable<int> estimatedTime { get; set; }
        public Nullable<int> idTime { get; set; }
        public Nullable<bool> isVisible { get; set; }
        public bool isActive { get; set; }
        public string typeIDs { get; set; }
        public string repeatIDs { get; set; }
        public string inventoryItemIDs { get; set; }
        public string goalIDs { get; set; }
        public string todoItemIDs { get; set; }
        public string taskIDs { get; set; }
        public string eventIDs { get; set; }
        public string goalIDs_TodoItem { get; set; }
        public string inventoryItemIDs_TodoItem { get; set; }
        public string taskIDs_TodoItem { get; set; }
        public string eventIDs_TodoItem { get; set; }
        public System.DateTime createdAt { get; set; }
        public System.DateTime updatedAt { get; set; }
    }
}