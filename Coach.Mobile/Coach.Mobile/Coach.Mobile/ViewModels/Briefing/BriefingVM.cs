using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using Coach.Mobile.Models.Briefing;
using Coach.Mobile.Models.Helper;
using Coach.Mobile.Models.InventoryItem;
using static Coach.Mobile.Models.Helper.TypeModel;
using static Coach.Mobile.Models.InventoryItem.InventoryItemModel;

namespace Coach.Mobile.ViewModels.Briefing
{
    public class BriefingVM : INotifyPropertyChanged
    {
        ObservableCollection<BriefingGroupVM> briefingGroups;

        public BriefingVM() : this(false) { }

        public BriefingVM(bool designData = false)
        {
            if (designData)
                this.CreateMockData();
        }

        public ObservableCollection<BriefingGroupVM> BriefingGroups
        {
            get { return this.briefingGroups; }
            set
            {
                briefingGroups = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("BriefingGroups"));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void CreateMockData()
        {
            RefreshBriefingGroups();
        }

        public async Task RefreshBriefingGroups()
        {
            var briefingGroups = new ObservableCollection<BriefingGroupVM>();
            var briefingModels = await BriefingModel.GetModelsAsync();

            var dates = briefingModels.Select(x => x.PostedAt.Date).Distinct();
            foreach (var date in dates.Select(x => x.Date))
            {
                var briefingGroup = new BriefingGroupVM(date, "Briefing");
                foreach (var briefing in briefingModels.Where(x => x.TypeID == (int)Types.Briefing && x.PostedAt.Date == date))
                {
                    briefingGroup.Add(briefing);
                }

                var deBriefingGroup = new BriefingGroupVM(date, "Debriefing");
                foreach (var deBriefing in briefingModels.Where(x => x.TypeID == (int)Types.Debriefing && x.PostedAt.Date == date))
                {
                    deBriefingGroup.Add(deBriefing);
                }

                if (briefingGroup.Count() > 0) briefingGroups.Add(briefingGroup);
                if (deBriefingGroup.Count() > 0) briefingGroups.Add(deBriefingGroup);
            }

            this.BriefingGroups = briefingGroups;
        }
    }

    public class BriefingGroupVM : ObservableCollection<BriefingModel>
    {
        static BriefingGroupVM()
        {
            Briefings = new List<BriefingGroupVM>();
        }

        public BriefingGroupVM(DateTime date, string briefingType)
        {
            this.Date = date;
            this.BriefingType = briefingType;
        }

        public DateTime Date { get; set; }
        public string BriefingType { get; set; }
        public static IList<BriefingGroupVM> Briefings { get; set; }
    }

    public class BriefingDetailPageVM : INotifyPropertyChanged
    {
        string toolbarButtonText;
        BriefingModel briefingModel;

        public BriefingDetailPageVM()
            : this(false)
        {

        }

        public BriefingDetailPageVM(bool designData = false)
        {
            if (designData)
                this.CreateMockData();
            else
            {
                this.BriefingModel = new BriefingModel()
                {
                    TypeID = (int)Types.Briefing,
                    PostedAt = DateTime.Now
                };
                this.BriefingEditModel = briefingModel;
            }
        }

        public BriefingDetailPageVM(BriefingModel briefingModel)
        {
            this.BriefingModel = briefingModel;
            this.BriefingEditModel = briefingModel.Clone();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public TypeModel SelectedType
        {
            get
            {
                return BriefingTypeModels.SingleOrDefault(x => x.RemoteID == this.BriefingModel.TypeID) ?? null;
            }
            set
            {
                this.BriefingModel.TypeID = value.RemoteID;
            }
        }
        public InventoryItemModel SelectedInventoryItem
        {
            get
            {
                return InventoryItemModels.SingleOrDefault(x => x.RemoteID == this.BriefingModel.InventoryItemID) ?? null;
            }
            set
            {
                this.BriefingModel.InventoryItemID = value.RemoteID;
            }
        }

        public string ToolbarButtonText // Text for the Save/Edit toolbar button
        {
            get { return this.toolbarButtonText; }
            set
            {
                toolbarButtonText = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("ToolbarButtonText"));
            }
        }

        public BriefingModel BriefingModel
        {
            get { return this.briefingModel; }
            set
            {
                briefingModel = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("BriefingModel"));
            }
        }
        public BriefingModel BriefingEditModel { get; set; } // Model used when page is in editing mode.

        public void CreateMockData()
        {
            this.BriefingModel = new BriefingModel
            {
                Text = "Today I'm going to go and spend Christmas at Mom's house",
                InventoryItemID = (int)InventoryItems.Social,
                PostedAt = new DateTime(2019, 12, 25)
            };
        }
    }
}
