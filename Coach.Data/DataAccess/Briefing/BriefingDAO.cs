using AutoMapper;
using Coach.Data.Extension;
using Coach.Data.Model;
using Coach.Model.Briefing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Coach.Data.Mappping.MyMapper;

namespace Coach.Data.DataAccess.Briefing
{
    public interface IBriefingDAO
    {
        List<BriefingModel> GetBriefings();
        List<BriefingModel> GetBriefingsForDate(DateTime date);
        int AddBriefing(BriefingModel briefingModel);
    }

    public class BriefingDAO : IBriefingDAO
    {
        coachdevEntities entities = new coachdevEntities();

        public List<BriefingModel> GetBriefings()
        {
            var briefings = entities.briefings.ToList();
            return CoachMapper.Map<List<BriefingModel>>(briefings);
        }

        public List<BriefingModel> GetBriefingsForDate(DateTime date)
        {
            var startDatetime = date.StartOfDay();
            var endDatetime = date.EndOfDay();

            var briefings = entities.briefings.Where(x => x.briefingDate >= startDatetime && x.briefingDate < endDatetime).ToList();
            return CoachMapper.Map<List<BriefingModel>>(briefings);
        }

        public int AddBriefing(BriefingModel briefingModel)
        {
            var briefing = CoachMapper.Map<briefing>(briefingModel);
            briefing = entities.briefings.Add(briefing);
            
            briefing.briefingDate = briefingModel.BriefingDate ?? DateTime.Now.Date; // If Briefing Date is null set it to current data
            briefing.postedAt = briefingModel.PostedAt ?? DateTime.Now; // If Posted At is null set it to current data time
            //briefing.isActive =  briefingModel.IsActive ?? 1; // If Is Active is null set it to true

            // Add Inventory Item mapping
            briefing.inventoryitem_briefing.Add(new inventoryitem_briefing
            {
                idInventoryItem = briefingModel.InventoryItemID
            });

            // Add Type mapping
            briefing.briefing_type.Add(new briefing_type
            {
                idType = briefingModel.TypeID
            });

            entities.SaveChanges();

            return briefing.id;
        }
    }
}
