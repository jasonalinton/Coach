using Coach.Data.EFExtention.Model.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coach.Data.Model
{
    public partial class logentry : ICoachDevModel, IMappingTable
    {
        public string text { get; set; }
        public string TableName => "logentry";
        public string DisplayName => "LogEntry";
        public string LeftTableName => "logentry";
        public string RightTableName => "logitem";
        public ICoachDevModel LeftTable => this;
        public ICoachDevModel RightTable => this.logitem;

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
