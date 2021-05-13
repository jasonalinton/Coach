using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coach.Data.EFExtention.Model.Interface
{
    public interface ICoachDevModel
    {
        int id { get; set; }
        string text { get; set; }
        string DisplayName {get;}
        string TableName { get; }
    }
}
