using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coach.Data
{
    public class LogAttribute : Attribute
    {
        public LogAttribute(TableTypes tableType)
        {
            this.TableType = tableType;
        }

        public TableTypes TableType { get; set; }
        public enum TableTypes
        {
            Main,
            Mapping, // Table that makes many:to:many relationships between two tables
            Left, // First/Main table mapped in a mapping table (first half of name ex. Left_Right)
            Right, // Second table mapped in a mapping table (name before underscore ex. Left_Right)
        }

    }
}
