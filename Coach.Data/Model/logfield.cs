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
    
    public partial class logfield
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public logfield()
        {
            this.logitem_field = new HashSet<logitem_field>();
        }
    
        public int id { get; set; }
        public int idType_Data { get; set; }
        public int idType_Control { get; set; }
        public string text { get; set; }
        public string json { get; set; }
        public sbyte isVisible { get; set; }
        public sbyte isActive { get; set; }
        public System.DateTime createdAt { get; set; }
        public System.DateTime updatedAt { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<logitem_field> logitem_field { get; set; }
        public virtual type type { get; set; }
        public virtual type type1 { get; set; }
    }
}