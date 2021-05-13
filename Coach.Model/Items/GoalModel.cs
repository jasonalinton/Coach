using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coach.Model.Items
{
    public class GoalModel
    {
        sbyte? isActive;
        sbyte? isVisible;

        public GoalModel()
        {
            this.ParentIDs = new List<int>();
            this.ChildIDs = new List<int>();

            this.InventoryItemIDs = new List<int>();
            this.RoutineIDs = new List<int>();
            this.TodoIDs = new List<int>();

            this.TypeIDs = new List<int>();

            //this.Time = new Time();

            this.TimeframeIDs = new List<int>();
            this.RepeatIDs = new List<int>();

            this.LocationIDs = new List<int>();
            this.PhotoIDs = new List<int>();
            this.NoteIDs = new List<int>();
            this.TagIDs = new List<int>();

            this.Types = new List<string>();

            this.Timeframes = new List<string>();
            //this.Repeats = new List<Repeat>();

        }

        public GoalModel(string goalString)
        {
            this.Text = goalString;
        }

        public int ID { get; set; }
        
        public string Text { get; set; }
        public string Explaination { get; set; }
        public string Deadline { get; set; }

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
        public List<int> TodoIDs { get; set; }

        //public Time Time { get; set; }

        /* Property Fields */
        public List<int> TypeIDs { get; set; }

        /* Time Fields */
        public List<int> TimeframeIDs { get; set; }
        public List<int> RepeatIDs { get; set; }

        /* Universal Fields */
        public List<int> LocationIDs { get; set; }
        public List<int> PhotoIDs { get; set; }
        public List<int> NoteIDs { get; set; }
        public List<int> TagIDs { get; set; }

        public List<string> Types { get; set; }

        public List<string> Timeframes { get; set; }
        //public List<Repeat> Repeats { get; set; }

        //public DateTime CreatedAt { get; set; }
        //public DateTime UpdatedAt { get; set; }

        public override string ToString()
        {
            return $"Goal: {this.Text}";
        }
    }
}
