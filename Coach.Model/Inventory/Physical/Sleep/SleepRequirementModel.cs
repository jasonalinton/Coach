using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coach.Model.Inventory.Physical.Sleep
{
    public class SleepRequirementModel
    {
        sbyte? isActive;
        sbyte? isVisible;

        public int ID { get; set; }
        public string Text
        {
            get { return $"You must be in bed by {Bedtime}, be up by {Awaketime}, and have {RequiredSleepHours} hours of sleep a night"; }
            set { }
        }
        public TimeSpan? Bedtime { get; set; }
        public TimeSpan? Awaketime { get; set; }
        public double? RequiredSleepHours { get; set; }
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
