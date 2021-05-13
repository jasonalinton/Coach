using Coach.Data.EFExtention.Model.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coach.Data.Model
{
    public partial class routine_eventtask : ICoachDevModel, IMappingTable
    {
        public string text { get; set; }
        public string TableName => "routine_eventtask";
        public string DisplayName => "Routine-EventTask";
        public string LeftTableName => "routine";
        public string RightTableName => "eventtask";
        public ICoachDevModel LeftTable => this.routine;
        public ICoachDevModel RightTable => this.eventtask;

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
