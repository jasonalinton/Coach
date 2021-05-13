using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coach.Model.Inventory.Physical.Sleep
{
    public class SleepSessionModel
    {
        sbyte? isActive;
        sbyte? isVisible;

        public SleepSessionModel()
        {
            Text = $"Sleep Session for {Date.ToShortDateString()} {Date.ToShortTimeString()}";
        }

        public int ID { get; set; }
        public string Text { get; set; }
        public DateTime Date { get; set; }
        public DateTime? TimeInBed { get; set; }
        public DateTime? TimeOutBed { get; set; }
        public DateTime? TimeAsleep { get; set; }
        public DateTime? TimeAwake { get; set; }
        public TimeSpan? Duration { get; set; }
        public TimeSpan? DurationAsleep { get; set; }
        public TimeSpan? DurationFallingAsleep { get; set; }
        public TimeSpan? DurationQualitySleep { get; set; }
        public TimeSpan? DurationDeepSleep { get; set; }
        public TimeSpan? DurationAwake { get; set; }
        public double? AverageHeartRate { get; set; }
        public double? WakingHeartRate { get; set; }
        public string JSON { get; set; }

        /// <summary>
        /// If set to null the model is automatically visible
        /// </summary>
        public sbyte? IsVisible
        {
            get
            {
                if (this.isVisible == null)
                    return 1;
                else
                    return isVisible;
            }
            set { this.isVisible = value; }
        }
        /// <summary>
        /// If set to null the model is automatically active
        /// </summary>
        public sbyte? IsActive
        {
            get
            {
                if (this.isActive == null)
                    return 1;
                else
                    return isActive;
            }
            set { this.isActive = value; }
        }
    }
}
