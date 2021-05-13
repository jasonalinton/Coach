using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coach.Data.EFExtention.Model.Interface
{
    public interface IMappingTable
    {
        string LeftTableName { get; }
        string RightTableName { get; }
        ICoachDevModel LeftTable { get; }
        ICoachDevModel RightTable { get; }
    }
}
