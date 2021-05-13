using Coach.Model.Planner.Time;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Coach.Model.Planner.Time.Types;

namespace Coach.Model.Items
{
    public class TodoModel
    {
        sbyte? isActive;
        sbyte? isVisible;

        public TodoModel()
        {
            this.TodoItemString = this.Text;

            this.ParentIDs = new List<int>();
            this.ChildIDs = new List<int>();

            this.InventoryItemIDs = new List<int>();
            this.RoutineIDs = new List<int>();
            this.GoalIDs = new List<int>();

            this.TypeIDs = new List<int>();
            this.MediumIDs = new List<int>();

            //this.Time = new Time();

            this.TimeframeIDs = new List<int>();
            this.RepeatIDs = new List<int>();

            this.LocationIDs = new List<int>();
            this.PhotoIDs = new List<int>();
            this.NoteIDs = new List<int>();
            this.TagIDs = new List<int>();

            this.Types = new List<string>();
            this.Mediums = new List<string>();

            //this.Timeframes = new List<string>();

            //this.Repeats = new List<Repeat>();

            this.Parents = new List<TodoModel>();
            this.EventTasks = new List<EventTaskModel>();
        }

        public TodoModel(string todoItemString)
        {
            this.TodoItemString = todoItemString;
            this.Text = todoItemString;
        }

        public int ID { get; set; }
        public int? Level { get; set; }
        public int? Points { get; set; }
        public int? EstimatedTime { get; set; }
        public int? RoutinePosition { get; set; }
        public int? ParentPosition { get; set; }
        public int? idDeadline { get; set; }
        public int? idTime { get; set; }

        public double? PercentComplete { get; set; }


        public string TodoItemString { get; set; }
        public string Text { get; set; }
        public string Deadline { get; set; }

        public sbyte IsGroup { get; set; }
        public sbyte? IsComplete { get; set; }

        /// <summary>
        /// If set to null the model is automatically visible
        /// </summary>
        public sbyte? IsVisible
        {
            get
            {
                if (this.isVisible == null)
                    return 1;
                else
                    return isVisible;
            }
            set { this.isVisible = value; }
        }
        /// <summary>
        /// If set to null the model is automatically active
        /// </summary>
        public sbyte? IsActive
        {
            get
            {
                if (this.isActive == null)
                    return 1;
                else
                    return isActive;
            }
            set { this.isActive = value; }
        }

        /* Family Fields */
        public List<int> ParentIDs { get; set; }
        public List<int> ChildIDs { get; set; }

        /* Item Fields */
        public List<int> InventoryItemIDs { get; set; }
        public List<int> RoutineIDs { get; set; }
        public List<int> GoalIDs { get; set; }

        //public Time Time { get; set; }

        public List<Time> Times { get; set; } = new List<Time>();
        public List<Repeat> Repeats { get; set; } = new List<Repeat>();
        public Time StartTime => Times.FirstOrDefault(x => x.EndPoint == Endpoints.Start && x.Type == TimeTypes.Actual);
        public Time EndTime => Times.FirstOrDefault(x => x.EndPoint == Endpoints.Start && x.Type == TimeTypes.Actual);

        /* Property Fields */
        public List<int> TypeIDs { get; set; }
        public List<int> MediumIDs { get; set; }
        public List<int> DeadlineIDs { get; set; }

        /* Time Fields */
        public List<int> TimeframeIDs { get; set; }
        public List<int> RepeatIDs { get; set; }

        /* Universal Fields */
        public List<int> LocationIDs { get; set; }
        public List<int> PhotoIDs { get; set; }
        public List<int> NoteIDs { get; set; }
        public List<int> TagIDs { get; set; }

        public List<string> Types { get; set; }
        public List<string> Mediums { get; set; }
        public List<string> Deadlines { get; set; }

        /* Sync Fields */
        //public DateTime CreatedAt { get; set; }
        //public DateTime UpdatedAt { get; set; }

        public List<InventoryItemModel> InventoryItems { get; set; }
        public List<GoalModel> Goals { get; set; }
        public List<RoutineModel> Routines { get; set; }
        public List<TodoModel> Parents { get; set; }
        public List<TodoModel> Children { get; set; }
        public List<EventTaskModel> EventTasks { get; set; }

        

        public override string ToString()
        {
            return $"TodoItem: {this.Text}";
        }
    }
}
