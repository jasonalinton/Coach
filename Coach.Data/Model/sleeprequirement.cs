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
    
    public partial class sleeprequirement
    {
        public int id { get; set; }
        public Nullable<System.TimeSpan> bedtime { get; set; }
        public Nullable<System.TimeSpan> awaketime { get; set; }
        public Nullable<double> requiredSleepHours { get; set; }
        public string json { get; set; }
        public sbyte isVisible { get; set; }
        public sbyte isActive { get; set; }
        public System.DateTime createdAt { get; set; }
        public System.DateTime updatedAt { get; set; }
    }
}