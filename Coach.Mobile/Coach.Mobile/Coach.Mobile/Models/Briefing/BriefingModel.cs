using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using SQLite;
using static Coach.Mobile.Models.Helper.TypeModel;
using static Coach.Mobile.Models.InventoryItem.InventoryItemModel;

namespace Coach.Mobile.Models.Briefing
{
    public class BriefingModel : ICoachModel, INotifyPropertyChanged
    {
        #region
        string text;
        DateTime postedAt;

        public event PropertyChangedEventHandler PropertyChanged;
        #endregion

        #region Constructors
        static BriefingModel()
        {
            Models = new List<ICoachModel>();
        }

        public BriefingModel()
        {

        }

        public BriefingModel(DateTime postedAt, Types type, string text, InventoryItems? inventoryItem = null)
            : this()
        {
            this.PostedAt = postedAt;
            this.TypeID = (int)type;
            this.Text = text;
            this.InventoryItemID = (inventoryItem != null) ? (int)inventoryItem : 0;
        }
        #endregion

        #region Datamembers
        [PrimaryKey, AutoIncrement]
        public int SQLiteID { get; set; }
        public int RemoteID { get; }
        public int ID { get; set; }
        public int TypeID { get; set; }
        public int InventoryItemID { get; set; }
        public string ModelName => "BriefingModel";
        public string Type => (this.TypeID != 0) ? Enum.GetName(typeof(Types), this.TypeID) : null;
        public string InventoryItem => (this.InventoryItemID != 0) ? Enum.GetName(typeof(InventoryItems), this.InventoryItemID) : null;
        public string Text
        {
            get { return this.text; }
            set
            {
                this.text = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Text"));
            }
        }
        public DateTime PostedAt
        {
            get { return this.postedAt; }
            set
            {
                this.postedAt = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("PostedAt"));
            }
        }
        #endregion

        #region Models
        public static List<ICoachModel> Models { get; set; }
        public static List<BriefingModel> BriefingModels =>  Models.Cast<BriefingModel>().ToList();
        #endregion

        #region Initializaion
        public static async Task InitializeModels()
        {
            Models = await App.Repository.GetModels("BriefingModel");

            if (Models.Count == 0)
            {
                Models.Add(new BriefingModel(new DateTime(2020, 1, 4), Types.Debriefing, "Today ended kinda weird cause I was hanging out with Michelle and Justin and I could tell when they eventually wanted me to leave", InventoryItems.Social));
                Models.Add(new BriefingModel(new DateTime(2019, 12, 25), Types.Briefing, "Today I'm going to go and spend Christmas at Mom's house", InventoryItems.Social));
                Models.Add(new BriefingModel(new DateTime(2019, 12, 25), Types.Briefing, "I'm gonna try to work on as much of the briefing part of my app as possible", InventoryItems.Financial));
                Models.Add(new BriefingModel(new DateTime(2020, 1, 4), Types.Briefing, "Working on my mobile app all day", InventoryItems.Financial));
                Models.Add(new BriefingModel(new DateTime(2019, 12, 25), Types.Debriefing, "My finances are wack, yo", InventoryItems.Financial));
                Models.Add(new BriefingModel(new DateTime(2020, 1, 11), Types.Debriefing, "Happy Birthday Brittany!!"));

                await App.Repository.AddModels(Models);

                Models = await App.Repository.GetModels("BriefingModel");
            }
        }
        #endregion

        #region Methods
        public static async Task<List<BriefingModel>> GetModelsAsync()
        {
            if (Models.Count < 1)
                await InitializeModels();
            else
                Models = await App.Repository.GetModels("BriefingModel");

            return BriefingModels;
        }

        public BriefingModel Clone()
        {
            /* Clone model */
            var serialized = JsonConvert.SerializeObject(this);
            return JsonConvert.DeserializeObject<BriefingModel>(serialized);
        }

        public async Task Save()
        {
            if (this.SQLiteID > 0)
                await App.Repository.UpdateModel(this);
            else
                await App.Repository.AddModel(this);
        }

        public void Delete()
        {
            App.Repository.DeleteModel(this);
        }
        #endregion
    }
}
