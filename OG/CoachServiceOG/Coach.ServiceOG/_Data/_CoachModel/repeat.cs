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
    
    public partial class repeat
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public repeat()
        {
            this.goal_repeat = new HashSet<goal_repeat>();
            this.repeat_dayofmonth = new HashSet<repeat_dayofmonth>();
            this.repeat_month = new HashSet<repeat_month>();
            this.repeat_dayofweek = new HashSet<repeat_dayofweek>();
            this.routine_repeat = new HashSet<routine_repeat>();
            this.todoitem_repeat = new HashSet<todoitem_repeat>();
        }
    
        public int idRepeat { get; set; }
        public int idTimeframe { get; set; }
        public int interval { get; set; }
        public int frequency { get; set; }
        public System.DateTime createdAt { get; set; }
        public System.DateTime updatedAt { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<goal_repeat> goal_repeat { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<repeat_dayofmonth> repeat_dayofmonth { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<repeat_month> repeat_month { get; set; }
        public virtual timeframe timeframe { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<repeat_dayofweek> repeat_dayofweek { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<routine_repeat> routine_repeat { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<todoitem_repeat> todoitem_repeat { get; set; }
    }
}
