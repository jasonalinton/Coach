using CoachModel._App._Task;
using System;
using System.Collections.Generic;
using System.Text;
using CoachModel._App._Time;
using CoachModel._Planner._Interface;

namespace CoachModel._Planner
{
    public class TodoItem : ITaskItem
    {
        public TodoItem()
        {
            this.TimeClass = new Time();
            
            this.MediumIDs = new List<int>();

            this.NoteIDs = new List<int>();
            this.TagIDs = new List<int>();

            this.Mediums = new List<string>();

            this.Repeats = new List<Repeat>();
        }

        public int ID { get { return this.idTodoItem; } }
        public int idTodoItem { get; set; }

        public int? Level { get; set; }
        public int? Points { get; set; }
        public int? EstimatedTime { get; set; }
        public int? Position { get; set; }
        public int? idDeadline { get; set; }
        public int? idTime { get; set; }

        public double? PercentComplete { get; set; }

        
        public string Title { get; set; }
        public string Text { get; set; }
        public string Description { get; set; }
        public string Deadline { get; set; }

        public bool IsGroup { get; set; }
        public bool? IsReviewable { get; set; }
        public bool? IsComplete { get; set; }
        public bool? IsVisible { get; set; }
        public bool? IsActive { get; set; }

        public Time TimeClass { get; set; }

        public string Types_Concat { get; set; }
        public string Mediums_Concat { get; set; }
        public string Timeframes_Concat { get; set; }

        public string TypeIDs_Concat { get; set; }
        public string TimeframeIDs_Concat { get; set; }
        public string RepeatIDs_Concat { get; set; }

        public string ParentIDs_Concat { get; set; }
        public string ChildIDs_Concat { get; set; }

        public string InventoryItemIDs_Concat { get; set; }
        public string GoalIDs_Concat { get; set; }
        public string RoutineIDs_Concat { get; set; }

        public string TaskIDs_Concat { get; set; }
        public string EventIDs_Concat { get; set; }

        /* Property Fields */
        public List<int> MediumIDs { get; }

        /* Universal Fields */
        public List<int> NoteIDs { get; }
        public List<int> TagIDs { get; }

        #region ID Lists
        /* Family Fields */
        public List<int> ParentIDs
        {
            get
            {
                if (this.ParentIDs_Concat != null)
                {
                    var stringList = new List<string>(this.ParentIDs_Concat.Split(','));
                    return stringList.ConvertAll(int.Parse); 
                }
                else
                {
                    return new List<int>();
                }
            }
        }
        public List<int> ChildIDs
        {
            get
            {
                if (this.ChildIDs_Concat != null)
                {
                    var stringList = new List<string>(this.ChildIDs_Concat.Split(','));
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

        /* Time Fields */
        public List<int> TimeframeIDs
        {
            get
            {
                if (this.TimeframeIDs_Concat != null)
                {
                    var stringList = new List<string>(this.TimeframeIDs_Concat.Split(','));
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
        public List<string> Timeframes
        {
            get
            {
                if (this.Timeframes_Concat != null)
                {
                    return new List<string>(this.Timeframes_Concat.Split(','));
                }
                else
                {
                    return new List<string>();
                }
            }
        }
        #endregion
        
        public List<string> Mediums { get; set; }

        public List<string> Repetitions
        {
            get
            {
                var repetitions = new List<string>();
                foreach (var repeat in this.Repeats)
                {
                    repetitions.Add(repeat.TimeframeClass.Repetition);
                }
                return repetitions;
            }
        }

        public List<Repeat> Repeats { get; set; }

        /* Sync Fields */
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public override string ToString()
        {
            return $"TodoItem: {this.Title}";
        }
    }
}
