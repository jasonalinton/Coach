using Coach.Data.EFExtention.Model.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coach.Data.Model
{
    public partial class eventtask : ICoachDevModel, IMainTable
    {
        public string TableName => "eventtask";
        public string DisplayName => "Event Task";

        public override string ToString()
        {
            return $"{this.DisplayName}: {this.text}";
        }
    }
}
