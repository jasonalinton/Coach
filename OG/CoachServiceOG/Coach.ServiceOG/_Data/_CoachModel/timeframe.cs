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
    
    public partial class timeframe
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public timeframe()
        {
            this.goal_timeframe = new HashSet<goal_timeframe>();
            this.inventoryitemspotlights = new HashSet<inventoryitemspotlight>();
            this.repeats = new HashSet<repeat>();
            this.todoitem_timeframe = new HashSet<todoitem_timeframe>();
        }
    
        public int idTimeframe { get; set; }
        public string timeframe1 { get; set; }
        public string timeframeAltText { get; set; }
        public string repetition { get; set; }
        public string repetitionAltText { get; set; }
        public System.DateTime createtdAt { get; set; }
        public System.DateTime updatedAt { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<goal_timeframe> goal_timeframe { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<inventoryitemspotlight> inventoryitemspotlights { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<repeat> repeats { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<todoitem_timeframe> todoitem_timeframe { get; set; }
    }
}