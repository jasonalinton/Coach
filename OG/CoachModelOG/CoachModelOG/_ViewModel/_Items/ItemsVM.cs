using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CoachModel._App._Helper;
using CoachModel._App._Universal;
using CoachModel._Inventory;
using CoachModel._Planner;

namespace CoachModel._ViewModel._Items
{
    public class ItemsVM
    {
        public ItemsVM()
        {
            this.InventoryItems = new List<InventoryItem>();

            this.Goals = new List<Goal>();
            this.TodoItems = new List<TodoItem>();
            this.Routines = new List<Routine>();

            this.Types = new List<_App._Universal.Type>();
            this.Mediums = new List<Medium>();
            this.Deadlines = new List<Deadline>();
            this.Timeframes = new List<_App._Time.Timeframe>();
        }

        public List<InventoryItem> InventoryItems { get; set; }

        public List<Goal> Goals { get; set; }
        public List<TodoItem> TodoItems { get; set; }
        public List<Routine> Routines { get; set; }

        public List<_App._Universal.Type> Types { get; set; }
        public List<Medium> Mediums { get; set; }
        public List<Deadline> Deadlines { get; set; }
        public List<_App._Time.Timeframe> Timeframes { get; set; }
    }
}

