using Coach.Data.EFExtention.Model.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coach.Data.Model
{
    public partial class logfield : ICoachDevModel, IMainTable
    {
        public string TableName => "logfield";
        public string DisplayName => "LogField";

        public override string ToString()
        {
            return $"{this.DisplayName}: {this.text}";
        }
    }
}
