using Coach.Data.EFExtention.Model.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coach.Data.Model
{
    public partial class goal : ICoachDevModel, IMainTable
    {
        public string TableName => "goal";
        public string DisplayName => "Goal";
        public List<int> InventoryItemIDs => new List<int>();
        public List<int> TodoIDs { get; set; } = new List<int>();
        public List<int> TimeIDs { get; set; } = new List<int>();

        public override string ToString()
        {
            return $"{this.DisplayName}: {this.text}";
        }
    }
}
