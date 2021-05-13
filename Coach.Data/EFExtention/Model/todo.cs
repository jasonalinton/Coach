using Coach.Data.EFExtention.Model.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coach.Data.Model
{
    public partial class todo : ICoachDevModel, IMainTable
    {
        public string TableName => "todo";
        public string DisplayName => "Todo";
        public List<int> InventoryItemIDs { get; set; } = new List<int>();
        public List<int> GoalIDs { get; set; } = new List<int>();
        public List<int> ParentIDs { get; set; } = new List<int>();
        public List<int> ChildIDs { get; set; } = new List<int>();
        public List<int> TimeIDs { get; set; } = new List<int>();
        public List<int> RepeatIDs { get; set; } = new List<int>();

        public override string ToString()
        {
            return $"{this.DisplayName}: {this.text}";
        }
    }
}