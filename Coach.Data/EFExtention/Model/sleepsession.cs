using Coach.Data.EFExtention.Model.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coach.Data.Model
{
    public partial class sleepsession : ICoachDevModel, IMainTable
    {
        public string text 
        { 
            get { return $"Sleep Session for {date.ToShortDateString()}"; }
            set { }
        }
        public string TableName => "sleepsession";
        public string DisplayName => "Sleep Session";

        public override string ToString()
        {
            return $"{this.DisplayName}: {this.text}";
        }
    }
}
