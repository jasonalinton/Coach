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
    
    public partial class briefing
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public briefing()
        {
            this.briefing_type = new HashSet<briefing_type>();
            this.inventoryitem_briefing = new HashSet<inventoryitem_briefing>();
        }
    
        public int id { get; set; }
        public string text { get; set; }
        public System.DateTime briefingDate { get; set; }
        public System.DateTime postedAt { get; set; }
        public string description { get; set; }
        public string json { get; set; }
        public string misc { get; set; }
        public sbyte isActive { get; set; }
        public System.DateTime createdAt { get; set; }
        public System.DateTime updatedAt { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<briefing_type> briefing_type { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<inventoryitem_briefing> inventoryitem_briefing { get; set; }
    }
}