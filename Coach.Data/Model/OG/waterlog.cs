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
    
    public partial class waterlog
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public waterlog()
        {
            this.meal_waterlog = new HashSet<meal_waterlog>();
        }
    
        public int idWaterLog { get; set; }
        public int amount { get; set; }
        public Nullable<System.DateTime> dateTime { get; set; }
        public System.DateTime createdAt { get; set; }
        public System.DateTime updatedAt { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<meal_waterlog> meal_waterlog { get; set; }
    }
}
