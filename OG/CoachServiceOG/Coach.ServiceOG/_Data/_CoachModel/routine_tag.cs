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
    
    public partial class routine_tag
    {
        public int idRoutine_Tag { get; set; }
        public int idRoutine { get; set; }
        public int idTag { get; set; }
        public Nullable<bool> isVisible { get; set; }
        public System.DateTime createdAt { get; set; }
        public System.DateTime updatedAt { get; set; }
    
        public virtual routine routine { get; set; }
        public virtual tag tag { get; set; }
    }
}
