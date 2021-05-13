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
    
    public partial class inventoryitem
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public inventoryitem()
        {
            this.inventoryitem_goal = new HashSet<inventoryitem_goal>();
            this.inventoryitem_logitem = new HashSet<inventoryitem_logitem>();
            this.inventoryitem_routine = new HashSet<inventoryitem_routine>();
            this.inventoryitem_todoitem = new HashSet<inventoryitem_todoitem>();
            this.inventoryitemspotlights = new HashSet<inventoryitemspotlight>();
        }
    
        public int idInventoryItem { get; set; }
        public string name { get; set; }
        public string title { get; set; }
        public string text { get; set; }
        public string description { get; set; }
        public Nullable<int> position { get; set; }
        public Nullable<bool> isVisible { get; set; }
        public bool isActive { get; set; }
        public System.DateTime createdAt { get; set; }
        public System.DateTime updatedAt { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<inventoryitem_goal> inventoryitem_goal { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<inventoryitem_logitem> inventoryitem_logitem { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<inventoryitem_routine> inventoryitem_routine { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<inventoryitem_todoitem> inventoryitem_todoitem { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<inventoryitemspotlight> inventoryitemspotlights { get; set; }
    }
}