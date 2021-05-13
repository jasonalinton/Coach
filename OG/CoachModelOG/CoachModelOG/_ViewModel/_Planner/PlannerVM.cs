using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CoachModel._App._Task;
using CoachModel._App._Time;
using CoachModel._Inventory;
using CoachModel._Planner;

namespace CoachModel._ViewModel._Planner
{
    public class PlannerVM
    {
        public PlannerVM()
        {
            this.InventoryItems = new List<InventoryItem>();
            this.SpotlightMappings = new List<SpotlightMapping>();

            this.Goals = new List<Goal>();
            this.TodoItems = new List<TodoItem>();
            this.Routines = new List<Routine>();

            this.Events = new List<Event>();
            this.Tasks = new List<Task>();

            this.Timeframes = new List<Timeframe>();
        }

        public List<InventoryItem> InventoryItems { get; set; }
        public List<SpotlightMapping> SpotlightMappings { get; set; }

        public List<Goal> Goals { get; set; }
        public List<TodoItem> TodoItems { get; set; }
        public List<Routine> Routines { get; set; }

        public List<Event> Events { get; set; }
        public List<Task> Tasks { get; set; }

        public List<Timeframe> Timeframes { get; set; }
    }
}
