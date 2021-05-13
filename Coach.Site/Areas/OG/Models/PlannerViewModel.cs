using CoachModel._App._Task;
using CoachModel._Inventory;
using CoachModel._Planner;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Coach.Site.Areas.OG.Models
{
    public class PlannerViewModel
    {
        public PlannerViewModel()
        {
            this.InventoryItems = new List<InventoryItem>();
            this.SpotlightMappings = new List<SpotlightMapping>();

            this.Goals = new List<Goal>();
            this.TodoItems = new List<TodoItem>();
            this.Routines = new List<Routine>();

            this.Events = new List<Event>();
            this.Tasks = new List<Task>();

            this.Repetitions = new List<string>();
        }

        public List<InventoryItem> InventoryItems { get; set; }
        public List<SpotlightMapping> SpotlightMappings { get; set; }

        public List<Goal> Goals { get; set; }
        public List<TodoItem> TodoItems { get; set; }
        public List<Routine> Routines { get; set; }

        public List<Event> Events { get; set; }
        public List<Task> Tasks { get; set; }

        public List<string> Repetitions { get; set; }
    }
}