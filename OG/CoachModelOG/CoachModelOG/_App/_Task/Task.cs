using System;
using System.Collections.Generic;
using System.Text;
using CoachModel._App._Time;

namespace CoachModel._App._Task
{
    public class Task
    {
        public Task()
        {
            this.TimeClass = new Time();
        }

        public int idTask { get; set; }
        public int idTime { get; set; }

        public string Title { get; set; }
        public string Text { get; set; }
        public string Timeframe { get; set; }
        public string Type { get; set; }

        public bool IsComplete { get; set; }
        public bool IsVisible { get; set; }
        public bool IsActive { get; set; }

        public string InventoryItemIDs_TodoItem_Concat { get; set; }
        public string InventoryItemIDs_Routine_Concat { get; set; }
        public string InventoryItemIDs_RoutineTodoItem_Concat { get; set; }

        public string GoalIDs_TodoItem_Concat { get; set; }
        public string GoalIDs_Routine_Concat { get; set; }
        public string GoalIDs_RoutineTodoItem_Concat { get; set; }

        public string TodoItemIDs_Concat { get; set; }
        public string TodoItemIDs_Routine_Concat { get; set; }
        public string TodoItemIDs_TodoItemRoutine_Concat { get; set; }

        public string RoutineIDs_Concat { get; set; }
        public string RoutineIDs_TodoItem_Concat { get; set; }

        public string EventIDs_Concat { get; set; }
        public string EventIDs_TodoItemRoutine_Concat { get; set; }

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
        public List<int> TodoItemIDs_TodoItemRoutine
        {
            get
            {
                if (this.TodoItemIDs_TodoItemRoutine_Concat != null)
                {
                    var stringList = new List<string>(this.TodoItemIDs_TodoItemRoutine_Concat.Split(','));
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
        public List<int> RoutineIDs_TodoItem
        {
            get
            {
                if (this.RoutineIDs_TodoItem_Concat != null)
                {
                    var stringList = new List<string>(this.RoutineIDs_TodoItem_Concat.Split(','));
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
        public List<int> EventIDs_TodoItemRoutine
        {
            get
            {
                if (this.EventIDs_TodoItemRoutine_Concat != null)
                {
                    var stringList = new List<string>(this.EventIDs_TodoItemRoutine_Concat.Split(','));
                    return stringList.ConvertAll(int.Parse);
                }
                else
                {
                    return new List<int>();
                }
            }
        }
        #endregion

        public Time TimeClass { get; set; }

        public DateTime StartDateTime { get; set; }
        public DateTime EndDateTime { get; set; }

        public string StartDateTime_String
        {
            get { return this.StartDateTime.ToString(); }
        }

        public string EndDateTime_String
        {
            get { return this.EndDateTime.ToString(); }
        }

        public DateTime DateTimeCompleted { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public override string ToString()
        {
            return $"Task: {this.Title}";
        }
    }
}
