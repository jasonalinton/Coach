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
    
    public partial class routine
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public routine()
        {
            this.routine_note = new HashSet<routine_note>();
            this.routine_repeat = new HashSet<routine_repeat>();
            this.goal_routine = new HashSet<goal_routine>();
            this.inventoryitem_routine = new HashSet<inventoryitem_routine>();
            this.routine_tag = new HashSet<routine_tag>();
            this.task_routine = new HashSet<task_routine>();
            this.routine_todoitem = new HashSet<routine_todoitem>();
            this.routine_type = new HashSet<routine_type>();
        }
    
        public int idRoutine { get; set; }
        public string title { get; set; }
        public string text { get; set; }
        public string description { get; set; }
        public Nullable<int> position { get; set; }
        public Nullable<int> estimatedTime { get; set; }
        public Nullable<int> idTime { get; set; }
        public Nullable<bool> isVisible { get; set; }
        public bool isActive { get; set; }
        public System.DateTime createdAt { get; set; }
        public System.DateTime updatedAt { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<routine_note> routine_note { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<routine_repeat> routine_repeat { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<goal_routine> goal_routine { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<inventoryitem_routine> inventoryitem_routine { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<routine_tag> routine_tag { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<task_routine> task_routine { get; set; }
        public virtual time time { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<routine_todoitem> routine_todoitem { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<routine_type> routine_type { get; set; }
    }
}
