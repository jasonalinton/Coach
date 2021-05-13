using Coach.Data.EFExtention.Model.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coach.Data.Model
{
    public partial class logitem : ICoachDevModel, IMainTable
    {
        public string TableName => "logitem";
        public string DisplayName => "LogItem";

        public override string ToString()
        {
            return $"{this.DisplayName}: {this.text}";
        }
    }
}
