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
    
    public partial class apptransaction
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public apptransaction()
        {
            this.apptransaction_errorlog = new HashSet<apptransaction_errorlog>();
            this.apptransaction_eventlog = new HashSet<apptransaction_eventlog>();
        }
    
        public int id { get; set; }
        public string json { get; set; }
        public sbyte isVisible { get; set; }
        public sbyte isActive { get; set; }
        public System.DateTime createdAt { get; set; }
        public System.DateTime updatedAt { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<apptransaction_errorlog> apptransaction_errorlog { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<apptransaction_eventlog> apptransaction_eventlog { get; set; }
    }
}