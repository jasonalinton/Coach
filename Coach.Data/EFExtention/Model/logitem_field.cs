using Coach.Data.EFExtention.Model.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coach.Data.Model
{
    public partial class logitem_field : ICoachDevModel, IMappingTable
    {
        public string text { get; set; }
        public string TableName => "logitem_field";
        public string DisplayName => "LogItem-LogField";
        public string LeftTableName => "logitem";
        public string RightTableName => "logfield";
        public ICoachDevModel LeftTable => this.logitem;
        public ICoachDevModel RightTable => this.logfield;

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
