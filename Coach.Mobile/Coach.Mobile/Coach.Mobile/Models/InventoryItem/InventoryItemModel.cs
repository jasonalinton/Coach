using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;

namespace Coach.Mobile.Models.InventoryItem
{
    public class InventoryItemModel : ICoachModel
    {

        #region
        #endregion

        #region Constructors
        public InventoryItemModel()
        {

        }
        static InventoryItemModel()
        {
            Models = new List<ICoachModel>();
        }

        public InventoryItemModel(InventoryItems inventoryItem)
            : this()
        {
            this.RemoteID = (int)inventoryItem;
        }
        #endregion

        #region Datamembers
        [PrimaryKey, AutoIncrement]
        public int SQLiteID { get; set; }
        public int RemoteID { get; }
        public string ModelName => "InventoryItemModel";
        public string Name => (this.RemoteID != 0) ? Enum.GetName(typeof(InventoryItems), this.RemoteID) : null;
        public string Description { get; set; }
        #endregion

        public static List<ICoachModel> Models { get; set; }
        public static List<InventoryItemModel> InventoryItemModels
        {
            get { return Models.Cast<InventoryItemModel>().ToList(); }
        }

        public enum InventoryItems
        {
            Emotional = 1,
            Mentoal = 2,
            Social = 3,
            Physical = 4,
            Financial = 5
        }

        #region Initializaion
        public static async Task InitializeModels()
        {
            Models = await App.Repository.GetModels("InventoryItemModel");

            if (Models.Count == 0)
            {
                Models.Add(new InventoryItemModel(InventoryItems.Emotional));
                Models.Add(new InventoryItemModel(InventoryItems.Mentoal));
                Models.Add(new InventoryItemModel(InventoryItems.Social));
                Models.Add(new InventoryItemModel(InventoryItems.Physical));
                Models.Add(new InventoryItemModel(InventoryItems.Financial));

                await App.Repository.AddModels(Models);

                Models = await App.Repository.GetModels("InventoryItemModel");
            }
        }
        #endregion

        #region Methods
        public static async Task<List<InventoryItemModel>> GetModelsAsync()
        {
            if (Models.Count < 1)
                await InitializeModels();
            else
                Models = await App.Repository.GetModels("InventoryItemModel");

            return InventoryItemModels;
        }
        #endregion
    }
}
