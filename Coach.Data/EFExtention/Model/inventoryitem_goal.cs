using Coach.Data.EFExtention.Model.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coach.Data.Model
{
    public partial class inventoryitem_goal : ICoachDevModel, IMappingTable
    {
        public string text { get; set; }
        public string TableName => "inventoryitem_goal";
        public string DisplayName => "InventoryItem-Goal";
        public string LeftTableName => "inventoryitem";
        public string RightTableName => "goal";
        public ICoachDevModel LeftTable => this.inventoryitem;
        public ICoachDevModel RightTable => this.goal;

        public override string ToString()
        {
            if (this.LeftTable != null && this.RightTable != null)
                return $"{this.DisplayName}: {this.LeftTable.text} :: {this.RightTable.text}";
            else if (this.LeftTable != null)
                return $"{this.DisplayName}: {this.LeftTable.text} :: {this.RightTableName}";
            else if (this.RightTable != null)
                return $"{this.DisplayName}: {this.LeftTableName} :: {this.RightTable.text}";

            return $"{this.DisplayName}";
        }
    }
}
