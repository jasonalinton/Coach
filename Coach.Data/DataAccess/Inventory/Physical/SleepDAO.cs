using Coach.Data.DataAccess.Logging;
using Coach.Data.Model;
using Coach.Model.Inventory.Physical.Sleep;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using static Coach.Data.Mappping.MyMapper;

namespace Coach.Data.DataAccess.Inventory.Physical
{
    public interface ISleepDAO
    {
        #region Sleep Session
        SleepSessionModel GetSleepSession(int idSleepSession);
        List<SleepSessionModel> GetSleepSessions();
        List<SleepSessionModel> GetSleepSessions(DateTime date, int range = 10);
        int AddSleepSession(SleepSessionModel sleepSessionModel);
        List<int> AddSleepSessions(List<SleepSessionModel> SleepSessionModels);
        List<int> AddAutoSleepData(List<AutoSleepModel> autoSleepModels);
        SleepSessionModel UpdateSleepSession(SleepSessionModel sleepSessionModel);
        List<SleepSessionModel> UpdateSleepSessions(List<SleepSessionModel> sleepSessionModels);
        void DeleteSleepSession(int id);
        void DeleteSleepSessions(List<int> ids);
        #endregion

        #region Sleep Requirement
        SleepRequirementModel GetSleepRequirement(int id = 0);
        List<SleepRequirementModel> GetSleepRequirements();
        int AddSleepRequirement(SleepRequirementModel sleepRequirementModel);
        List<int> AddSleepRequirements(List<SleepRequirementModel> SleepRequirementModels);
        SleepRequirementModel UpdateSleepRequirement(SleepRequirementModel sleepRequirementModel);
        List<SleepRequirementModel> UpdateSleepRequirements(List<SleepRequirementModel> sleepRequirementModels);
        void DeleteSleepRequirement(int id);
        void DeleteSleepRequirements(List<int> ids);
        #endregion
    }
    public class SleepDAO : ISleepDAO
    {
        #region Sleep Session
        public SleepSessionModel GetSleepSession(int idSleepSession)
        {
            using (coachdevEntities entities = new coachdevEntities())
            {
                var sleepsession = entities.sleepsessions.SingleOrDefault(x => x.id == idSleepSession);
                return CoachMapper.Map<SleepSessionModel>(sleepsession);
            }
        }

        public List<SleepSessionModel> GetSleepSessions()
        {
            using (coachdevEntities entities = new coachdevEntities())
            {
                var sleepsessions = entities.sleepsessions.ToList();
                return CoachMapper.Map<List<SleepSessionModel>>(sleepsessions);
            }
        }

        public List<SleepSessionModel> GetSleepSessions(DateTime date, int range = 10)
        {
            var startDate = date.AddDays(-range);
            var endDate = date.AddDays(-range);

            using (coachdevEntities entities = new coachdevEntities())
            {
                var sleepsessions = entities.sleepsessions.Where(x => x.date >= startDate && x.date <= endDate).ToList();
                return CoachMapper.Map<List<SleepSessionModel>>(sleepsessions);
            }
        }

        public int AddSleepSession(SleepSessionModel sleepSessionModel)
        {
            using (coachdevEntities entities = new coachdevEntities())
            {
                var sleepsession = CoachMapper.Map<sleepsession>(sleepSessionModel);
                entities.sleepsessions.Add(sleepsession);

                entities.SaveChanges();

                return sleepsession.id;
            }
        }

        public List<int> AddSleepSessions(List<SleepSessionModel> SleepSessionModels)
        {
            var newIDs = new List<int>();

            foreach (var sleepSessionModel in SleepSessionModels)
            {
                try
                {
                    newIDs.Add(AddSleepSession(sleepSessionModel));
                }
                catch (Exception ex)
                {
                    ex.Data.Add("logMessage", $"Error adding sleepsession: \"{sleepSessionModel.Text}");
                    LogDAO.AddQueuedLogError(ex);
                }
            }

            return newIDs;
        }

        /// <summary>
        /// Add data recieved from the AutoSleep app
        /// </summary>
        /// <param name="autoSleepModels"></param>
        /// <returns></returns>
        public List<int> AddAutoSleepData(List<AutoSleepModel> autoSleepModels)
        {
            var ids = new List<int>();
            var sleepsessions = CoachMapper.Map<List<sleepsession>>(autoSleepModels);
            var newSessions = new List<sleepsession>();

            using (coachdevEntities entities = new coachdevEntities())
            {
                foreach (var sleepsession in sleepsessions)
                {
                    var sessionDate = sleepsession.date.Date;
                    var sessionToUpdate = entities.sleepsessions.Where(x => x.date == sessionDate).ToList();

                    if (sessionToUpdate.Count() > 0)
                    {
                        sleepsession.id = sessionToUpdate[0].id;
                        entities.Entry(sessionToUpdate[0]).CurrentValues.SetValues(sleepsession);
                        entities.Entry(sessionToUpdate[0]).State = EntityState.Modified;

                        if (sessionToUpdate.Count() > 1)
                            LogDAO.LogInfo($"There are duplicate sleep sessions for {sleepsession.date}", sleepsession.id, "sleepsession", sleepsession.date.ToShortDateString(), "WANING");
                    }
                    else
                    {
                        newSessions.Add(sleepsession);
                        entities.sleepsessions.Add(sleepsession);
                    }

                }

                entities.SaveChanges();
            }

            return newSessions.Select(x => x.id).ToList();
        }

        public SleepSessionModel UpdateSleepSession(SleepSessionModel sleepSessionModel)
        {
            using (coachdevEntities entities = new coachdevEntities())
            {
                var originalsleepsession = entities.sleepsessions.SingleOrDefault(x => x.id == sleepSessionModel.ID);
                if (originalsleepsession != null)
                {
                    var sleepsession = CoachMapper.Map<sleepsession>(sleepSessionModel);

                    entities.Entry(originalsleepsession).CurrentValues.SetValues(sleepsession);
                    entities.Entry(originalsleepsession).State = EntityState.Modified;
                    entities.SaveChanges();

                    return CoachMapper.Map<SleepSessionModel>(originalsleepsession);
                }
                else
                    return null;
            }
        }

        public List<SleepSessionModel> UpdateSleepSessions(List<SleepSessionModel> sleepSessionModels)
        {
            if (sleepSessionModels == null) return null;

            using (coachdevEntities entities = new coachdevEntities())
            {
                var updatedModels = new List<SleepSessionModel>();

                foreach (var sleepSessionModel in sleepSessionModels)
                    updatedModels.Add(this.UpdateSleepSession(sleepSessionModel));

                return updatedModels;
            }
        }

        public void DeleteSleepSession(int id)
        {
            if (id <= 0) return;

            using (coachdevEntities entities = new coachdevEntities())
            {
                var sleepsession = entities.sleepsessions.SingleOrDefault(x => x.id == id);

                if (sleepsession != null)
                {
                    entities.sleepsessions.Remove(sleepsession);
                    entities.SaveChanges();
                }
            }
        }

        public void DeleteSleepSessions(List<int> ids)
        {
            if (ids == null) return;

            using (coachdevEntities entities = new coachdevEntities())
            {
                var sleepsessions = entities.sleepsessions.Where(x => ids.Contains(x.id));

                entities.sleepsessions.RemoveRange(sleepsessions);
                entities.SaveChanges();
            }
        }
        #endregion

        #region Sleep Requirement
        /// <summary>
        /// Get sleep requirement.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Returns SleepRequirement with matching ID. If no ID is provided, return the last active record</returns>
        public SleepRequirementModel GetSleepRequirement(int id = 0)
        {
            using (coachdevEntities entities = new coachdevEntities())
            {
                var sleeprequirement = (id > 0) ? 
                    entities.sleeprequirements.ToList().SingleOrDefault(x => x.id == id) :
                    entities.sleeprequirements.ToList().LastOrDefault(x => x.isActive == 1);

                return CoachMapper.Map<SleepRequirementModel>(sleeprequirement);
            }
        }

        public List<SleepRequirementModel> GetSleepRequirements()
        {
            using (coachdevEntities entities = new coachdevEntities())
            {
                var sleeprequirements = entities.sleeprequirements.ToList();
                return CoachMapper.Map<List<SleepRequirementModel>>(sleeprequirements);
            }
        }

        public int AddSleepRequirement(SleepRequirementModel sleepRequirementModel)
        {
            using (coachdevEntities entities = new coachdevEntities())
            {
                var sleeprequirement = CoachMapper.Map<sleeprequirement>(sleepRequirementModel);
                entities.sleeprequirements.Add(sleeprequirement);

                entities.SaveChanges();

                return sleeprequirement.id;
            }
        }

        public List<int> AddSleepRequirements(List<SleepRequirementModel> SleepRequirementModels)
        {
            var newIDs = new List<int>();

            foreach (var sleepRequirementModel in SleepRequirementModels)
            {
                try
                {
                    newIDs.Add(AddSleepRequirement(sleepRequirementModel));
                }
                catch (Exception ex)
                {
                    ex.Data.Add("logMessage", $"Error adding sleeprequirement: \"{sleepRequirementModel.Text}");
                    LogDAO.AddQueuedLogError(ex);
                }
            }

            return newIDs;
        }

        public SleepRequirementModel UpdateSleepRequirement(SleepRequirementModel sleepRequirementModel)
        {
            using (coachdevEntities entities = new coachdevEntities())
            {
                var originalsleepsession = entities.sleeprequirements.SingleOrDefault(x => x.id == sleepRequirementModel.ID);
                if (originalsleepsession != null)
                {
                    var sleeprequirement = CoachMapper.Map<sleeprequirement>(sleepRequirementModel);

                    entities.Entry(originalsleepsession).CurrentValues.SetValues(sleeprequirement);
                    entities.Entry(originalsleepsession).State = EntityState.Modified;
                    entities.SaveChanges();

                    return CoachMapper.Map<SleepRequirementModel>(originalsleepsession);
                }
                else
                    return null;
            }
        }

        public List<SleepRequirementModel> UpdateSleepRequirements(List<SleepRequirementModel> sleepRequirementModels)
        {
            if (sleepRequirementModels == null) return null;

            using (coachdevEntities entities = new coachdevEntities())
            {
                var updatedModels = new List<SleepRequirementModel>();

                foreach (var sleepRequirementModel in sleepRequirementModels)
                    updatedModels.Add(this.UpdateSleepRequirement(sleepRequirementModel));

                return updatedModels;
            }
        }

        public void DeleteSleepRequirement(int id)
        {
            if (id <= 0) return;

            using (coachdevEntities entities = new coachdevEntities())
            {
                var sleeprequirement = entities.sleeprequirements.SingleOrDefault(x => x.id == id);

                if (sleeprequirement != null)
                {
                    entities.sleeprequirements.Remove(sleeprequirement);
                    entities.SaveChanges();
                }
            }
        }

        public void DeleteSleepRequirements(List<int> ids)
        {
            if (ids == null) return;

            using (coachdevEntities entities = new coachdevEntities())
            {
                var sleeprequirements = entities.sleeprequirements.Where(x => ids.Contains(x.id));

                entities.sleeprequirements.RemoveRange(sleeprequirements);
                entities.SaveChanges();
            }
        } 
        #endregion
    }
}
