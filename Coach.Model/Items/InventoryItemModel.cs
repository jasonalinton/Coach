using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coach.Model.Items
{
    public class InventoryItemModel
    {
        public InventoryItemModel()
        {
            this.GoalIDs = new List<int>();
            this.TodoItemIDs = new List<int>();
            this.RoutineIDs = new List<int>();

            //this.Goals = new List<Goal>();
            //this.TodoItems = new List<TodoItem>();
            //this.Routines = new List<Routine>();
        }

        public InventoryItemModel(int id, string item)
            : this()
        {
            this.ID = id;
            this.Item = item;
            this.Text = item;
        }

        public int ID { get; set; }
        public string Item { get; set; }
        public string Text { get; set; }

        public List<int> GoalIDs { get; set; }
        public List<int> TodoItemIDs { get; set; }
        public List<int> RoutineIDs { get; set; }

        //public List<Goal> Goals { get; set; }
        //public List<TodoItem> TodoItems { get; set; }
        //public List<Routine> Routines { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public enum InventoryItems
        {
            Physical = 1,
            Social = 2,
            Mental = 3,
            Emotional = 4,
            Financial = 5
        }

        public override string ToString()
        {
            return $"Inventory Item: {this.Text}";
        }

    }
}
