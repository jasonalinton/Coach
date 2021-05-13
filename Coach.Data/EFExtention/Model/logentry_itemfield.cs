using Coach.Data.EFExtention.Model.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coach.Data.Model
{
    public partial class logentry_itemfield : ICoachDevModel, IMappingTable
    {
        public string text { get; set; }
        public string TableName => "logentry_itemfield";
        public string DisplayName => "LogEntry-LogItemField";
        public string LeftTableName => "logentry";
        public string RightTableName => "logitem_field";
        public ICoachDevModel LeftTable => this.logentry;
        public ICoachDevModel RightTable => this.logitem_field;

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
