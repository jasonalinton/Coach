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
    
    public partial class routine_note
    {
        public int idRoutine_Note { get; set; }
        public int idRoutine { get; set; }
        public int idNote { get; set; }
        public Nullable<bool> isVisible { get; set; }
        public System.DateTime createdAt { get; set; }
        public System.DateTime updatedAt { get; set; }
    
        public virtual routine routine { get; set; }
        public virtual note note { get; set; }
    }
}
