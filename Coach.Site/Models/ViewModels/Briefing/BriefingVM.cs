using Coach.Model.Briefing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using static Coach.Model.Helper.TypeModel;
using static Coach.Model.Items.InventoryItemModel;

namespace Coach.Site.Models.ViewModels.Briefing
{
    public class BriefingVM
    {
        public BriefingVM(List<BriefingModel> briefingModels, DateTime date)
        {
            var briefings = new List<Briefing>();
            foreach (var briefingModel in briefingModels.OrderByDescending(x => x.PostedAt).OrderBy(x => x.InventoryItemID))
                briefings.Add(new Briefing(briefingModel, date));

            Briefings = briefings.Where(x => x.TypeID == (int)Types.Briefing).ToList();
            Debriefings = briefings.Where(x => x.TypeID == (int)Types.Debriefing).ToList();
        }

        public List<Briefing> Briefings { get; set; }
        public List<Briefing> Debriefings { get; set; }
    }

    public class Briefing
    {
        public Briefing(BriefingModel briefingModel, DateTime date)
        {
            InventoryItemID = briefingModel.InventoryItemID;
            TypeID = briefingModel.TypeID;
            Text = briefingModel.Text;
            BriefingDate = (DateTime)briefingModel.BriefingDate;
            PostedAt = (DateTime)briefingModel.PostedAt;
        }

        public int? ID { get; set; }
        public int TypeID { get; set; }
        public int InventoryItemID { get; set; }
        public string InventoryItem => (InventoryItemID != 0) ? Enum.GetName(typeof(InventoryItems), InventoryItemID) : null;
        public string Type => (TypeID != 0) ? Enum.GetName(typeof(Types), TypeID) : null;
        public string Text { get; set; }
        public string Date => PostedAt.ToLongDateString();
        public string Time => PostedAt.ToShortTimeString();
        public DateTime BriefingDate { get; set; }
        public DateTime PostedAt { get; set; }
    }
}