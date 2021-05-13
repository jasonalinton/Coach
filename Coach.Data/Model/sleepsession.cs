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
    
    public partial class sleepsession
    {
        public int id { get; set; }
        public System.DateTime date { get; set; }
        public Nullable<System.DateTime> timeInBed { get; set; }
        public Nullable<System.DateTime> timeOutBed { get; set; }
        public Nullable<System.DateTime> timeAsleep { get; set; }
        public Nullable<System.DateTime> timeAwake { get; set; }
        public Nullable<System.TimeSpan> duration { get; set; }
        public Nullable<System.TimeSpan> durationAsleep { get; set; }
        public Nullable<System.TimeSpan> durationFallingAsleep { get; set; }
        public Nullable<System.TimeSpan> durationQualitySleep { get; set; }
        public Nullable<System.TimeSpan> durationDeepSleep { get; set; }
        public Nullable<System.TimeSpan> durationAwake { get; set; }
        public Nullable<double> wakingHeartRate { get; set; }
        public Nullable<double> averageHeartRate { get; set; }
        public string json { get; set; }
        public sbyte isVisible { get; set; }
        public sbyte isActive { get; set; }
        public System.DateTime createdAt { get; set; }
        public System.DateTime updatedAt { get; set; }
    }
}
