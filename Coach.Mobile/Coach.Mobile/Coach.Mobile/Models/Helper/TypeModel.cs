using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using SQLite;

namespace Coach.Mobile.Models.Helper
{
    public class TypeModel : ICoachModel, INotifyPropertyChanged
    {
        #region
        public event PropertyChangedEventHandler PropertyChanged;
        #endregion

        #region Constructors
        static TypeModel()
        {
            Models = new List<ICoachModel>();
        }

        public TypeModel()
        {

        }

        public TypeModel(Types type, Types parentType)
            : this()
        {
            this.RemoteID = (int)type;
            this.ParentTypeID = (int)parentType;
        }
        #endregion

        #region Datamembers
        [PrimaryKey, AutoIncrement]
        public int SQLiteID { get; set; }
        public int RemoteID { get; }
        public int ParentTypeID { get; set; }
        public string ModelName => "TypeModel";
        public string Type => (this.RemoteID != 0) ? Enum.GetName(typeof(Types), this.RemoteID) : null;
        public string ParentType => (this.ParentTypeID != 0) ? Enum.GetName(typeof(Types), this.ParentTypeID) : null;
        #endregion

        #region Models
        public static List<ICoachModel> Models { get; set; }
        public static List<TypeModel> TypeModels => Models.Cast<TypeModel>().ToList();
        public static List<TypeModel> BriefingTypeModels
        {
            get { return TypeModels.Where(x => x.ParentTypeID == (int)Types.BriefingType).ToList(); }
        }
        #endregion

        public enum Types
        {
            BriefingType = 1,
            Briefing = 2,
            Debriefing = 3
        }

        #region Initializaion
        public static async Task InitializeModels()
        {
            Models = await App.Repository.GetModels("TypeModel");

            if (Models.Count == 0)
            {
                Models.Add(new TypeModel(Types.Briefing, Types.BriefingType));
                Models.Add(new TypeModel(Types.Debriefing, Types.BriefingType));

                await App.Repository.AddModels(Models);

                Models = await App.Repository.GetModels("TypeModel");
            }
        }
        #endregion

        #region Methods
        public static async Task<List<TypeModel>> GetModelsAsync()
        {
            if (Models.Count < 1) 
                await InitializeModels();
            else 
                Models = await App.Repository.GetModels("TypeModel");

            return TypeModels;
        }
        #endregion
    }
}
