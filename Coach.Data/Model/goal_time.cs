//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Coach.Data.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class goal_time
    {
        public int id { get; set; }
        public int idGoal { get; set; }
        public int idTime { get; set; }
        public string description { get; set; }
        public string json { get; set; }
        public string misc { get; set; }
        public Nullable<int> position { get; set; }
        public sbyte isVisible { get; set; }
        public sbyte isActive { get; set; }
        public System.DateTime createdAt { get; set; }
        public System.DateTime updatedAt { get; set; }
    
        public virtual goal goal { get; set; }
        public virtual time time { get; set; }
    }
}
