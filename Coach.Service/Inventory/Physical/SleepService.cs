using Coach.Data.DataAccess.Inventory.Physical;
using Coach.Model.Inventory.Physical.Sleep;
using CsvHelper;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Coach.Service.Inventory.Physical
{
    public interface ISleepService
    {
        #region Sleep Session
        SleepSessionModel GetSleepSession(int idSleepSession);
        List<SleepSessionModel> GetSleepSessions();
        int AddSleepSession(SleepSessionModel sleepSessionModel);
        List<int> AddSleepSessions(List<SleepSessionModel> SleepSessionModels);
        SleepSessionModel UpdateSleepSession(SleepSessionModel sleepSessionModel);
        List<SleepSessionModel> UpdateSleepSessions(List<SleepSessionModel> sleepSessionModels);
        void DeleteSleepSession(int id);
        void DeleteSleepSessions(List<int> ids);
        void UploadAutoSleepData(HttpPostedFileBase csvFile);
        #endregion

        #region Sleep Requirement
        SleepRequirementModel GetSleepRequirement();
        SleepRequirementModel GetSleepRequirement(int id);
        List<SleepRequirementModel> GetSleepRequirements();
        int AddSleepRequirement(SleepRequirementModel sleepRequirementModel);
        List<int> AddSleepRequirements(List<SleepRequirementModel> SleepRequirementModels);
        SleepRequirementModel UpdateSleepRequirement(SleepRequirementModel sleepRequirementModel);
        List<SleepRequirementModel> UpdateSleepRequirements(List<SleepRequirementModel> sleepRequirementModels);
        void DeleteSleepRequirement(int id);
        void DeleteSleepRequirements(List<int> ids);
        #endregion

    }
    public class SleepService : ISleepService
    {
        ISleepDAO _sleepDAO;

        public SleepService(ISleepDAO sleepDAO)
        {
            _sleepDAO = sleepDAO;
        }

        #region Sleep Session
        public SleepSessionModel GetSleepSession(int idSleepSession)
        {
            return _sleepDAO.GetSleepSession(idSleepSession);
        }

        public List<SleepSessionModel> GetSleepSessions()
        {
            return _sleepDAO.GetSleepSessions();
        }

        public List<SleepSessionModel> GetSleepSessions(DateTime middleDate)
        {
            return _sleepDAO.GetSleepSessions(middleDate);
        }

        public int AddSleepSession(SleepSessionModel sleepSessionModel)
        {
            return _sleepDAO.AddSleepSession(sleepSessionModel);
        }

        public List<int> AddSleepSessions(List<SleepSessionModel> SleepSessionModels)
        {
            return _sleepDAO.AddSleepSessions(SleepSessionModels);
        }

        public SleepSessionModel UpdateSleepSession(SleepSessionModel sleepSessionModel)
        {
            return _sleepDAO.UpdateSleepSession(sleepSessionModel);
        }

        public List<SleepSessionModel> UpdateSleepSessions(List<SleepSessionModel> sleepSessionModels)
        {
            return _sleepDAO.UpdateSleepSessions(sleepSessionModels);
        }

        public void DeleteSleepSession(int id)
        {
            _sleepDAO.DeleteSleepSession(id);
        }

        public void DeleteSleepSessions(List<int> ids)
        {
            _sleepDAO.DeleteSleepSessions(ids);
        }

        /// <summary>
        /// Upload data from the AutoSleep app's exported CSV file
        /// </summary>
        /// <param name="csvFile"></param>
        public void UploadAutoSleepData(HttpPostedFileBase csvFile)
        {
            var autoSleepModels = new List<AutoSleepModel>();

            /* Conver CSV file to AutoSleepModel */
            using (StreamReader reader = new StreamReader(csvFile.InputStream))
            {
                using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
                {
                    /* Configure CSVReader */
                    csv.Configuration.HeaderValidated = null; // Model properties don't need to match CSV header fields
                    csv.Configuration.MissingFieldFound = null; // Model doesn't have to include all fields in CSV

                    var records = csv.GetRecords<AutoSleepModel>();
                    autoSleepModels = records.ToList();
                }
            }

            _sleepDAO.AddAutoSleepData(autoSleepModels);
        }
        #endregion


        #region Sleep Requirement
        public SleepRequirementModel GetSleepRequirement()
        {
            return _sleepDAO.GetSleepRequirement();
        }
        public SleepRequirementModel GetSleepRequirement(int id)
        {
            return _sleepDAO.GetSleepRequirement(id);
        }

        public List<SleepRequirementModel> GetSleepRequirements()
        {
            return _sleepDAO.GetSleepRequirements();
        }

        public int AddSleepRequirement(SleepRequirementModel sleepRequirementModel)
        {
            return _sleepDAO.AddSleepRequirement(sleepRequirementModel);
        }

        public List<int> AddSleepRequirements(List<SleepRequirementModel> SleepRequirementModels)
        {
            return _sleepDAO.AddSleepRequirements(SleepRequirementModels);
        }

        public SleepRequirementModel UpdateSleepRequirement(SleepRequirementModel sleepRequirementModel)
        {
            return _sleepDAO.UpdateSleepRequirement(sleepRequirementModel);
        }

        public List<SleepRequirementModel> UpdateSleepRequirements(List<SleepRequirementModel> sleepRequirementModels)
        {
            return _sleepDAO.UpdateSleepRequirements(sleepRequirementModels);
        }

        public void DeleteSleepRequirement(int id)
        {
            _sleepDAO.DeleteSleepRequirement(id);
        }

        public void DeleteSleepRequirements(List<int> ids)
        {
            _sleepDAO.DeleteSleepRequirements(ids);
        }
        #endregion
    }
}
