using Coach.Data.EFExtention.Model.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coach.Data.Model
{
    public partial class inventoryitem : ICoachDevModel
    {
        public string TableName => "inventoryitem";
        public string DisplayName => "InventoryItem";
        public List<int> GoalIDs => new List<int>();
        public List<int> TodoIDs => new List<int>();

        public override string ToString()
        {
            return $"{this.DisplayName}: {this.text}";
        }
    }
}
