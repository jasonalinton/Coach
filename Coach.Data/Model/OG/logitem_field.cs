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
    
    public partial class logitem_field
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public logitem_field()
        {
            this.logentry_itemfield = new HashSet<logentry_itemfield>();
        }
    
        public int idLogItem_Field { get; set; }
        public int idLogItem { get; set; }
        public int idLogField { get; set; }
        public Nullable<int> position { get; set; }
        public int active { get; set; }
    
        public virtual logitem logitem { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<logentry_itemfield> logentry_itemfield { get; set; }
        public virtual logfield logfield { get; set; }
    }
}
