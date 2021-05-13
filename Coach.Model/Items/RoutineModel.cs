using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coach.Model.Items
{
    public class RoutineModel
    {
        sbyte? isActive;
        sbyte? isVisible;

        public RoutineModel()
        {
            this.ParentIDs = new List<int>();
            this.ChildIDs = new List<int>();

            this.InventoryItemIDs = new List<int>();
            this.GoalIDs = new List<int>();
            this.TodoIDs = new List<int>();

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

            this.Timeframes = new List<string>();

            //this.Repeats = new List<Repeat>();

            this.Todos = new List<TodoModel>();
            this.EventTasks = new List<EventTaskModel>();
        }

        public RoutineModel(string routineSting)
            : this()
        {
            this.Text = routineSting;
        }

        public int ID { get; set; }
        public string RoutineString { get; set; }
        public string Text { get; set; }
        public string Explaination { get; set; }

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
        public List<int> GoalIDs { get; set; }
        public List<int> TodoIDs { get; set; }

        //public Time Time { get; set; }

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

        public List<string> Timeframes { get; set; }
        //public List<Repeat> Repeats { get; set; }

        /* Sync Fields */
        //public DateTime CreatedAt { get; set; }
        //public DateTime UpdatedAt { get; set; }

        public List<EventTaskModel> EventTasks { get; set; }

        public List<TodoModel> Todos { get; set; }

        public override string ToString()
        {
            return $"Routine: {this.Text}";
        }
    }
}
