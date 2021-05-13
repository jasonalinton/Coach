using Coach.Data.EFExtention.Model.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Coach.Model.Planner.Time.Types;

namespace Coach.Data.Model
{
    public partial class repeat : ICoachDevModel
    {
        public string TableName => "repeat";
        public string DisplayName => "Repeat";
        public string text
        {
            get => $"{frequency} times every {interval} {((Timeframes)idTimeframe).ToString()}";
            set { }
        }
        public List<int> TimeIDs { get; set; } = new List<int>();

        public override string ToString() => $"{this.DisplayName}: {this.text}";
    }
}
