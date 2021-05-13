using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Coach.Data.Extension;
using Coach.Data.Model;
using Coach.Model.Inventory.Physical.Sleep;
using Google.Protobuf.WellKnownTypes;
using Microsoft.Ajax.Utilities;
using MySqlX.XDevAPI;
using Renci.SshNet.Security.Cryptography.Ciphers.Modes;

namespace Coach.Site.Models.ViewModels.Sleep
{
    public class SleepVM
    {
        public SleepVM()
        {
            SleepEvents = new List<SleepEvent>();
            SleepSessions = new List<SleepSessionModel>();
        }

        public SleepVM(List<SleepSessionModel> sleepSessions, SleepRequirementModel sleepRequirements)
            : this()
        {
            SleepEvents = InitializeSleepEvents(sleepSessions, sleepRequirements);
            CurrentSleepEvent = SleepEvents.LastOrDefault();

            SleepRequirements = sleepRequirements;
            SleepSessions = sleepSessions;
        }

        public SleepVM(List<SleepSessionModel> sleepSessions, SleepRequirementModel sleepRequirements, DateTime date)
            : this()
        {
            SleepEvents = InitializeSleepEvents(sleepSessions, sleepRequirements);
            var sleepEvents = SleepEvents.Where(x => ((DateTime)x.Date).Date == date.Date).ToList();
            if (sleepEvents.Count > 0)
                CurrentSleepEvent = sleepEvents[0];

            SleepRequirements = sleepRequirements;
            SleepSessions = sleepSessions;
        }

        public List<SleepEvent> InitializeSleepEvents(List<SleepSessionModel> sleepSessions, SleepRequirementModel sleepRequirements)
        {
            SleepEvents = new List<SleepEvent>();
            foreach (var sleepSession in sleepSessions)
                SleepEvents.Add(new SleepEvent(sleepSession, sleepRequirements));

            return SleepEvents;
        }

        public SleepEvent CurrentSleepEvent { get; set; }
        public List<SleepEvent> SleepEvents { get; set; }
        public SleepRequirementModel SleepRequirements { get; set; }
        public List<SleepSessionModel> SleepSessions { get; set; }
    }

    public class SleepEvent
    {
        public SleepEvent(SleepSessionModel sleepSessionModel, SleepRequirementModel sleepRequirements)
        {
            SleepRequirements = sleepRequirements;
            SleepSessionModel = sleepSessionModel;

            Date = sleepSessionModel?.Date.Date;
            AsleepDuration = sleepSessionModel.DurationAsleep;
        }

        public SleepRequirementModel SleepRequirements { get; set; }
        public SleepSessionModel SleepSessionModel { get; set; }
        /// <summary>
        /// The date the session ends and gets analyzed for. Not the date that ended before the session
        /// </summary>
        public DateTime? Date { get; set; }
        public TimeSpan? AsleepDuration { get; set; }
        public string SessionDuration
        {
            get
            {
                if (SleepSessionModel?.Duration != null)
                {
                    var sessionDuration = (TimeSpan)SleepSessionModel.Duration;
                    return $"{sessionDuration.Hours}h {sessionDuration.Minutes}m";
                }
                else
                    return "--h --m";
            }
        }
        public string PercentReuiredSleep
        {
            get
            {
                if (AsleepDuration != null && SleepRequirements?.RequiredSleepHours != null)
                {
                    var asleepDuration = ((TimeSpan)AsleepDuration).TotalSeconds;
                    var requiredSleepDuration = new TimeSpan((int)SleepRequirements.RequiredSleepHours, 0, 0).TotalSeconds;

                    return ((int)(asleepDuration / requiredSleepDuration * 100)).ToString();
                }
                else
                    return "--";
            }
        }
        public string TimeInBed
        {
            get
            {
                if (SleepSessionModel?.TimeInBed != null)
                    return ((DateTime)SleepSessionModel.TimeInBed).ToShortTimeString();
                else
                    return "--:--";
            }
        }

        /// <summary>
        /// Time required to be awake by
        /// </summary>
        public  string TimeAwake
        {
            get
            {
                if (SleepSessionModel?.TimeAwake != null)
                    return ((DateTime)SleepSessionModel.TimeAwake).ToShortTimeString();
                else
                    return "--:--";
            }
        }

        /// <summary>
        /// Time required to be in bed by
        /// </summary>
        public DateTime? BedTime
        {
            get
            {
                if (SleepRequirements?.Bedtime != null)
                {
                    var bedtime = (TimeSpan)SleepRequirements.Bedtime;
                    var sessionBedtime = SleepSessionModel.Date.AddDays(-1).Date.AddSeconds(bedtime.TotalSeconds);
                    return (bedtime.Hours > 12) ? sessionBedtime : sessionBedtime.AddDays(1);
                }
                else
                    return SleepSessionModel.Date.AddDays(-1).EndOfDay(); // Default Bedtime is 11:59:59
            }
        }

        public DateTime? Awaketime
        {
            get
            {
                if (SleepRequirements?.Awaketime != null)
                {
                    var awaketime = (TimeSpan)SleepRequirements.Awaketime;
                    return SleepSessionModel.Date.Date.AddSeconds(awaketime.TotalSeconds);
                }
                else
                {
                    var defaultWaketime = new TimeSpan(8, 0, 0); // Default Bedtime is 8:00:00
                    return SleepSessionModel.Date.Date.AddSeconds(defaultWaketime.TotalSeconds);
                }
            }
        }
        public bool? SessionDurationSuccess
        {
            get
            {
                if (SleepSessionModel?.Duration != null && SleepRequirements?.RequiredSleepHours != null)
                {
                    var sessionDuration = ((TimeSpan)SleepSessionModel.Duration).TotalSeconds;
                    var requiredSleepDuration = new TimeSpan((int)SleepRequirements.RequiredSleepHours, 0, 0).TotalSeconds;

                    return (sessionDuration >= requiredSleepDuration);
                }
                else
                    return null;
            }
        }

        public  bool? InBedSuccess
        {
            get
            {
                if (SleepRequirements?.Bedtime == null || SleepSessionModel?.TimeInBed == null)
                    return null;

                if (SleepSessionModel.TimeInBed <= BedTime)
                    return true;
                else
                    return false;
            }
        }

        public  bool? OutBedSuccess
        {
            get
            {
                if (SleepRequirements?.Awaketime == null || SleepSessionModel?.TimeAwake == null)
                    return null;

                if (SleepSessionModel.TimeAwake <= Awaketime)
                    return true;
                else
                    return false;
            }
        }

        public string SessionDurationSuccessClass
        {
            get
            {
                if (SessionDurationSuccess != null)
                    return ((bool)SessionDurationSuccess) ? "success" : "failure";
                else
                    return "no-data";
            }
        }

        public string InBedSuccessClass
        {
            get
            {
                if (InBedSuccess != null)
                    return ((bool)InBedSuccess) ? "fas fa-check complete" : "fas fa-times incomplete";
                else
                    return "fas fa-circle no-data";
            }
        }

        public string OutBedSuccessClass
        {
            get
            {
                if (OutBedSuccess != null)
                    return ((bool)OutBedSuccess) ? "fas fa-check complete" : "fas fa-times incomplete";
                else
                    return "fas fa-circle no-data";
            }
        }
    }
}