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
    
    public partial class type_type
    {
        public int id { get; set; }
        public int idType { get; set; }
        public int idParent { get; set; }
        public Nullable<int> position { get; set; }
        public string json { get; set; }
        public sbyte isVisible { get; set; }
        public sbyte isActive { get; set; }
        public System.DateTime createdAt { get; set; }
        public System.DateTime updatedAt { get; set; }
    
        public virtual type type { get; set; }
        public virtual type type1 { get; set; }
    }
}
