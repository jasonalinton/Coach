using System;
using System.Collections.Generic;
using System.Text;

namespace CoachModel._App._Task
{
    public class Event
    {
        public Event()
        {
            this.Timeframes = new List<string>();
            this.Types = new List<string>();
        }

        public int idEvent { get; set; }

        public int OwnerID
        {
            get
            {
                if (this.Type == "Routine_Complete")
                    return 1;
                else if (this.Type == "Routine_Incomplete")
                    return 2;
                else if (this.Type == "Task_Complete")
                    return 3;
                else if (this.Type == "Task_Incomplete")
                    return 4;
                else if (this.Type == "Routine_Single")
                    return 5;
                else if (this.Type == "Routine_Repetitive")
                    return 6;
                else if (this.Type == "TodoItem_Spotlight")
                    return 7;
                else if (this.Type == "TodoItem_Errand")
                    return 8;
                else if (this.Type == "TodoItem_Tally")
                    return 9;
                else if (this.Type == "TodoItem_Inverse")
                    return 10;
                else if (this.Type == "TodoItem_Repetitive")
                    return 11;
                else if (this.Type == "TodoItem_Spotlight_Repetitive")
                    return 12;
                else if (this.Type == "TodoItem_Errand_Repetitive")
                    return 13;
                else if (this.Type == "TodoItem_Tally_Repetitive")
                    return 14;
                else if (this.Type == "TodoItem_Inverse_Repetitive")
                    return 15;
                else
                    return -1;
            }
            set { }
        }

        public int PercentComplete { get; set; }

        public string Title { get; set; }
        public string Text { get; set; }
        public string Type { get; set; }
        public string EventType
        {
            get
            {
                if (Title == "Breakfast" || Text == "Breakfast" ||
                    Title == "Brunch" || Text == "Brunch" ||
                    Title == "Lunch" || Text == "Lunch" ||
                    Title == "Linner" || Text == "Linner" ||
                    Title == "Dinner" || Text == "Dinner" ||
                    Title == "Snack" || Text == "Snack")
                {
                    return "meal-event";
                }
                else
                    return "no-data";
            }
        }
        public int MealID { get; set; }
        public string Timezone { get; set; }
        public string Repeat { get; set; }
        public string Description { get; set; }

        public bool? IsAllDay { get; set; }
        public bool? IsVisible { get; set; }
        public bool? IsActive { get; set; }

        public DateTime? StartDateTime { get; set; }
        public DateTime? EndDateTime { get; set; }

        public List<string> Timeframes { get; set; }
        public List<string> Types { get; set; }

        #region ID Lists Concatenated
        public string InventoryItemIDs_TodoItem_Concat { get; set; }
        public string InventoryItemIDs_Routine_Concat { get; set; }
        public string InventoryItemIDs_RoutineTodoItem_Concat { get; set; }

        public string GoalIDs_TodoItem_Concat { get; set; }
        public string GoalIDs_Routine_Concat { get; set; }
        public string GoalIDs_RoutineTodoItem_Concat { get; set; }

        public string TodoItemIDs_Concat { get; set; }
        public string TodoItemIDs_Routine_Concat { get; set; }

        public string RoutineIDs_Concat { get; set; }

        public string TaskIDs_Concat { get; set; }
        public string MealIDs_Concat { get; set; }
        #endregion

        #region ID Lists
        public List<int> InventoryItemIDs_TodoItem
        {
            get
            {
                if (this.InventoryItemIDs_TodoItem_Concat != null)
                {
                    var stringList = new List<string>(this.InventoryItemIDs_TodoItem_Concat.Split(','));
                    return stringList.ConvertAll(int.Parse);
                }
                else
                {
                    return new List<int>();
                }
            }
        }
        public List<int> InventoryItemIDs_Routine
        {
            get
            {
                if (this.InventoryItemIDs_Routine_Concat != null)
                {
                    var stringList = new List<string>(this.InventoryItemIDs_Routine_Concat.Split(','));
                    return stringList.ConvertAll(int.Parse);
                }
                else
                {
                    return new List<int>();
                }
            }
        }
        public List<int> InventoryItemIDs_RoutineTodoItem
        {
            get
            {
                if (this.InventoryItemIDs_RoutineTodoItem_Concat != null)
                {
                    var stringList = new List<string>(this.InventoryItemIDs_RoutineTodoItem_Concat.Split(','));
                    return stringList.ConvertAll(int.Parse);
                }
                else
                {
                    return new List<int>();
                }
            }
        }

        public List<int> GoalIDs_TodoItem
        {
            get
            {
                if (this.GoalIDs_TodoItem_Concat != null)
                {
                    var stringList = new List<string>(this.GoalIDs_TodoItem_Concat.Split(','));
                    return stringList.ConvertAll(int.Parse);
                }
                else
                {
                    return new List<int>();
                }
            }
        }
        public List<int> GoalIDs_Routine
        {
            get
            {
                if (this.GoalIDs_Routine_Concat != null)
                {
                    var stringList = new List<string>(this.GoalIDs_Routine_Concat.Split(','));
                    return stringList.ConvertAll(int.Parse);
                }
                else
                {
                    return new List<int>();
                }
            }
        }
        public List<int> GoalIDs_RoutineTodoItem
        {
            get
            {
                if (this.GoalIDs_RoutineTodoItem_Concat != null)
                {
                    var stringList = new List<string>(this.GoalIDs_RoutineTodoItem_Concat.Split(','));
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
                if (this.TodoItemIDs_Concat != null)
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
        public List<int> TodoItemIDs_Routine
        {
            get
            {
                if (this.TodoItemIDs_Routine_Concat != null)
                {
                    var stringList = new List<string>(this.TodoItemIDs_Routine_Concat.Split(','));
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

        public List<int> MealIDs
        {
            get
            {
                if (this.MealIDs_Concat != null)
                {
                    var stringList = new List<string>(this.MealIDs_Concat.Split(','));
                    return stringList.ConvertAll(int.Parse);
                }
                else
                {
                    return new List<int>();
                }
            }
        }
        #endregion

        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public override string ToString()
        {
            return $"Event: {this.Title}";
        }
    }

    public class Events
    {
        public List<Event> EventList { get; set; }
    }
}
