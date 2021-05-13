using Coach.Model.Helper;
using Coach.Model.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Coach.Model.Helper.TypeModel;
using static Coach.Model.Items.InventoryItemModel;

namespace Coach.Model.Briefing
{
    public class BriefingModel
    {
        sbyte? isActive;
        sbyte? isVisible;

        public BriefingModel()
        {
            InventoryItems = new List<InventoryItemModel>();
            Types = new List<TypeModel>();
        }

        #region Datamembers
        //[PrimaryKey, AutoIncrement]
        //public int SQLiteID { get; set; }
        public int? ID { get; set; }
        public int RemoteID { get; }
        public int TypeID { get; set; }
        public int InventoryItemID { get; set; }
        public string ModelName => "BriefingModel";
        public string Type => (this.TypeID != 0) ? 
            Enum.GetName(typeof(Types), this.TypeID) : null;
        public string InventoryItem => (this.InventoryItemID != 0) ? 
            Enum.GetName(typeof(InventoryItems), this.InventoryItemID) : null;
        public string Text { get; set; }

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
        public DateTime? BriefingDate { get; set; }
        public DateTime? PostedAt { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public List<InventoryItemModel> InventoryItems { get; set; }
        public List<TypeModel> Types { get; set; }
        #endregion
    }
}
