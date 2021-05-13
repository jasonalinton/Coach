using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoachModel._Planner._Interface
{
    public interface ITaskItem
    {
        int ID { get; }
        string Title { get; set; }
    }
}
