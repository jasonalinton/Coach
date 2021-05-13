using Coach.Data.DataAccess.Briefing;
using Coach.Model.Briefing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coach.Service.Briefing
{
    public interface IBriefingService
    {
        List<BriefingModel> GetBriefingForDate(DateTime date);
        void AddBriefing(BriefingModel briefing);
    }
    public class BriefingService : IBriefingService
    {
        IBriefingDAO _briefingDAO;

        public BriefingService(IBriefingDAO briefingDAO)
        {
            _briefingDAO = briefingDAO;
        }

        public void AddBriefing(BriefingModel briefing)
        {
            _briefingDAO.AddBriefing(briefing);
        }

        public List<BriefingModel> GetBriefingForDate(DateTime date)
        {
            return _briefingDAO.GetBriefingsForDate(date);
        }
    }
}
