using System;
using System.Collections.Generic;
using System.Text;

namespace CoachModel._Inventory
{
    public class InventoryItem
    {
        public InventoryItem()
        {
            //this.Goals = new List<Goal>();
            //this.TodoItems = new List<TodoItem>();
            //this.Routines = new List<Routine>();
        }

        public int idInventoryItem { get; set; }

        public string Title { get; set; }
        public string Text { get; set; }

        public bool isVisible { get; set; }
        public bool isActive { get; set; }
        
        public string GoalIDs_Concat { get; set; }
        public string TodoItemIDs_Concat { get; set; }
        public string RoutineIDs_Concat { get; set; }

        public string TaskIDs_Concat { get; set; }
        public string EventIDs_Concat { get; set; }

        #region ID Lists
        public List<int> GoalIDs
        {
            get
            {
                if (this.GoalIDs_Concat != null)
                {
                    var stringList = new List<string>(this.GoalIDs_Concat.Split(','));
                    return stringList.ConvertAll(int.Parse);
                }
                else
                {
                    return new List<int>();
                }
            }
        }
        public List<int> RoutineIDs
        {
            get
            {
                if (this.RoutineIDs_Concat != null)
                {
                    var stringList = new List<string>(this.RoutineIDs_Concat.Split(','));
                    return stringList.ConvertAll(int.Parse);
                }
                else
                {
                    return new List<int>();
                }
            }
        }
        public List<int> TodoItemIDs
        {
            get
            {
                if (this.RoutineIDs_Concat != null)
                {
                    var stringList = new List<string>(this.TodoItemIDs_Concat.Split(','));
                    return stringList.ConvertAll(int.Parse);
                }
                else
                {
                    return new List<int>();
                }
            }
        }

        public List<int> TaskIDs
        {
            get
            {
                if (this.TaskIDs_Concat != null)
                {
                    var stringList = new List<string>(this.TaskIDs_Concat.Split(','));
                    return stringList.ConvertAll(int.Parse);
                }
                else
                {
                    return new List<int>();
                }
            }
        }
        public List<int> EventIDs
        {
            get
            {
                if (this.EventIDs_Concat != null)
                {
                    var stringList = new List<string>(this.EventIDs_Concat.Split(','));
                    return stringList.ConvertAll(int.Parse);
                }
                else
                {
                    return new List<int>();
                }
            }
        }
        #endregion

        //public List<Goal> Goals { get; set; }
        //public List<TodoItem> TodoItems { get; set; }
        //public List<Routine> Routines { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public override string ToString()
        {
            return $"Inventory Item: {this.Title}";
        }

    }
}
