using CsvHelper.Configuration.Attributes;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coach.Model.Inventory.Physical.Sleep
{
    public class AutoSleepModel
    {
        [Name("ISO8601")]
        public string ISO8601 { get; set; }
        public DateTime Date => Convert.ToDateTime(ISO8601).Date;
        [Name("fromDate")]
        public string FromDate { get; set; }
        [Name("toDate")]
        public string ToDate { get; set; }
        [Name("bedtime")]
        public string Bedtime { get; set; }
        [Name("waketime")]
        public string Waketime { get; set; }
        [Name("inBed")]
        public string InBed { get; set; }
        [Name("awake")]
        public string Awake { get; set; }
        [Name("fellAsleepIn")]
        public string FellAsleepIn { get; set; }
        [Name("sessions")]
        public string Sessions { get; set; }
        [Name("asleep")]
        public string Asleep { get; set; }
        [Name("asleepAvg7")]
        public string AsleepAvg7 { get; set; }
        [Name("efficiency")]
        public string Efficiency { get; set; }
        [Name("efficiencyAvg7")]
        public string EfficiencyAvg7 { get; set; }
        [Name("quality")]
        public string Quality { get; set; }
        [Name("qualityAvg7")]
        public string QualityAvg7 { get; set; }
        [Name("deep")]
        public string Deep { get; set; }
        [Name("deepAvg7")]
        public string DeepAvg7 { get; set; }
        [Name("sleepBPM")]
        public string SleepBPM { get; set; }
        [Name("sleepBPMAvg7")]
        public string SleepBPMAvg7 { get; set; }
        [Name("dayBPM")]
        public string DayBPM { get; set; }
        [Name("dayBPMAvg7")]
        public string DayBPMAvg7 { get; set; }
        [Name("wakingBPM")]
        public string WakingBPM { get; set; }
        [Name("wakingBPMAvg7")]
        public string WakingBPMAvg7 { get; set; }
        [Name("hrv")]
        public string HRV { get; set; }
        [Name("hrvAvg7")]
        public string HRVAvg7 { get; set; }
        [Name("tags")]
        public string Tags { get; set; }
        [Name("notes")]
        public string Notes { get; set; }
        public sbyte isVisible => 1;
        public sbyte isActive => 1;

    }
}
