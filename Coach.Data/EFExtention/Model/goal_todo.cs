using Coach.Data.EFExtention.Model.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Coach.Data.Model
{
    public partial class goal_todo : ICoachDevModel, IMappingTable
    {
        public string text { get; set; }
        public string TableName => "goal_todo";
        public string DisplayName => "Goal-Todo";
        public string LeftTableName => "goal";
        public string RightTableName => "todo";
        public ICoachDevModel LeftTable => this.goal;
        public ICoachDevModel RightTable => this.todo;

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