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
    
    public partial class inventoryitemspotlight
    {
        public int idInventoryItemSpotlight { get; set; }
        public int idInventoryItem { get; set; }
        public int idTimeframe { get; set; }
        public System.DateTime datetime { get; set; }
    
        public virtual inventoryitem inventoryitem { get; set; }
        public virtual timeframe timeframe { get; set; }
    }
}
