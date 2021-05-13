using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CoachModel._App._Time;

namespace CoachModel._Inventory
{
    public class SpotlightMappingVM
    {
        public SpotlightMappingVM()
        {
            this.SpotlightMappings = new List<SpotlightMapping>();
        }

        public List<SpotlightMapping> SpotlightMappings { get; set; }
    }

    public class SpotlightMapping
    {
        public SpotlightMapping()
        {
            this.TimeframeClass = new Timeframe();
        }

        public int idInventoryItem { get; set; }
        public string InventoryItemString { get; set; }
        public Timeframe TimeframeClass { get; set; }
        public DateTime DateTime { get; set; }

        public string DateTimeString
        {
            get
            {
                return this.DateTime.ToString();
            }
        }

    }
}
