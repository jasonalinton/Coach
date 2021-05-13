using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coach.Model.Helper
{
    public class TypeModel
    {
        public TypeModel()
        {
            Parents = new List<Type>();
            Children = new List<Type>();
        }

        #region Datamembers
        //[PrimaryKey, AutoIncrement]
        //public int SQLiteID { get; set; }
        public int? ID { get; set; }
        public int RemoteID { get; }
        public int ParentTypeID { get; set; }
        public string ModelName => "TypeModel";
        public string Text { get; set; }
        public string AltText { get; set; }
        public string Description { get; set; }
        public string JSON { get; set; }
        public string Type => (this.RemoteID != 0) ? Enum.GetName(typeof(Types), this.RemoteID) : null;
        public string ParentType => (this.ParentTypeID != 0) ? Enum.GetName(typeof(Types), this.ParentTypeID) : null;
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public List<Type> Parents { get; set; }
        public List<Type> Children { get; set; }
        #endregion

        #region Enums
        public enum Types
        {
            BriefingType = 100,
            Briefing = 101,
            Debriefing = 102
        }
        #endregion
    }
}
