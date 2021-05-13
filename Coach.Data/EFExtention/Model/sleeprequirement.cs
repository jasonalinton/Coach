using Coach.Data.EFExtention.Model.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coach.Data.Model
{
    public partial class sleeprequirement : ICoachDevModel, IMainTable
    {
        public sleeprequirement()
        {
            text = $"You must be in bed by {bedtime}, be up by {awaketime}, and have {requiredSleepHours} hours of sleep a night";
        }

        public string text { get; set; }
        public string TableName => "sleeprequirement";
        public string DisplayName => "Sleep Requirement";

        public override string ToString()
        {
            return $"{this.DisplayName}: {this.text}";
        }
    }
}