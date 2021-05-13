using CoachModel._App._Task;
using System;
using System.Collections.Generic;
using System.Text;
using CoachModel._App._Time;
using CoachModel._Planner._Interface;

namespace CoachModel._Planner
{
    public class Routine : ITaskItem
    {
        public Routine()
        {
            this.TimeClass = new Time();

            this.NoteIDs = new List<int>();
            this.TagIDs = new List<int>();

            this.Repeats = new List<Repeat>();
        }

        public int ID { get { return this.idRoutine; } }
        public int idRoutine { get; set; }

        public int? idTime { get; set; }
        public int? EstimatedTime { get; set; }
        
        public string Title { get; set; }
        public string Text { get; set; }
        public string Description { get; set; }

        public bool? IsVisible { get; set; }
        public bool? IsActive { get; set; }

        public Time TimeClass { get; set; }

        public string Types_Concat { get; set; }
        public string Repetitions_Concat { get; set; }
        public string TypeIDs_Concat { get; set; }
        public string RepeatIDs_Concat { get; set; }

        public string InventoryItemIDs_Concat { get; set; }
        public string InventoryItemIDs_TodoItem_Concat { get; set; }
        public string GoalIDs_Concat { get; set; }
        public string GoalIDs_TodoItem_Concat { get; set; }
        public string TodoItemIDs_Concat { get; set; }

        public string TaskIDs_Concat { get; set; }
        public string TaskIDs_TodoItem_Concat { get; set; }
        public string EventIDs_Concat { get; set; }
        public string EventIDs_TodoItem_Concat { get; set; }

        /* Universal Fields */
        public List<int> NoteIDs { get; }
        public List<int> TagIDs { get; }

        #region ID Lists
        /* Property Fields */
        public List<string> Types
        {
            get
            {
                if (this.Types_Concat != null)
                {
                    return new List<string>(this.Types_Concat.Split(','));
                }
                else
                {
                    return new List<string>();
                }
            }
        }
        public List<int> TypeIDs
        {
            get
            {
                if (this.TypeIDs_Concat != null)
                {
                    var stringList = new List<string>(this.TypeIDs_Concat.Split(','));
                    return stringList.ConvertAll(int.Parse);
                }
                else
                {
                    return new List<int>();
                }
            }
        }
        public List<int> RepeatIDs
        {
            get
            {
                if (this.RepeatIDs_Concat != null)
                {
                    var stringList = new List<string>(this.RepeatIDs_Concat.Split(','));
                    return stringList.ConvertAll(int.Parse);
                }
                else
                {
                    return new List<int>();
                }
            }
        }
        public List<int> InventoryItemIDs
        {
            get
            {
                if (this.InventoryItemIDs_Concat != null)
                {
                    var stringList = new List<string>(this.InventoryItemIDs_Concat.Split(','));
                    return stringList.ConvertAll(int.Parse);
                }
                else
                {
                    return new List<int>();
                }
            }
        }
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
        public List<int> TaskIDs_TodoItem
        {
            get
            {
                if (this.TaskIDs_TodoItem_Concat != null)
                {
                    var stringList = new List<string>(this.TaskIDs_TodoItem_Concat.Split(','));
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
        public List<int> EventIDs_TodoItem
        {
            get
            {
                if (this.EventIDs_TodoItem_Concat != null)
                {
                    var stringList = new List<string>(this.EventIDs_TodoItem_Concat.Split(','));
                    return stringList.ConvertAll(int.Parse);
                }
                else
                {
                    return new List<int>();
                }
            }
        }

        public List<string> Repetitions
        {
            get
            {
                if (this.Repetitions_Concat != null)
                {
                    return new List<string>(this.Repetitions_Concat.Split(','));
                }
                else
                {
                    return new List<string>();
                }
            }
        }
        #endregion


        public List<Repeat> Repeats { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public override string ToString()
        {
            return $"Routine: {this.Title}";
        }
    }
}
