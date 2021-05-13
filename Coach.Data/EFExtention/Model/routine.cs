using Coach.Data.EFExtention.Model.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coach.Data.Model
{
    public partial class routine : ICoachDevModel, IMainTable
    {
        public string TableName => "routine";
        public string DisplayName => "Routine";
        public List<int> TimeIDs { get; set; } = new List<int>();
        public List<int> RepeatIDs { get; set; } = new List<int>();

        public override string ToString()
        {
            return $"{this.DisplayName}: {this.text}";
        }
    }
}
