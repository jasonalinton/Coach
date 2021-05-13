using System;
using System.Collections.Generic;
using System.Text;

namespace CoachModel._App._Time
{
    public class Time
    {
        /* Start */
        public TimeSpan StartTime { get; set; }
        public DateTime StartDate { get; set; }
        public string StartDayOfWeek { get; set; }
        public int? StartDayOfMonth { get; set; }
        public DateTime StartWeek { get; set; }
        public DateTime StartMonth { get; set; }
        public DateTime StartYear { get; set; }

        /* End */
        public TimeSpan EndTime { get; set; }
        public DateTime EndDate { get; set; }
        public string EndDayOfWeek { get; set; }
        public int? EndDayOfMonth { get; set; }
        public DateTime EndWeek { get; set; }
        public DateTime EndMonth { get; set; }
        public DateTime EndYear { get; set; }

        /* Recommended Start */
        public TimeSpan RecommendedStartTime { get; set; }
        public DateTime RecommendedStartDate { get; set; }
        public string RecommendedStartDayOfWeek { get; set; }
        public int? RecommendedStartDayOfMonth { get; set; }
        public DateTime RecommendedStartWeek { get; set; }
        public DateTime RecommendedStartMonth { get; set; }
        public DateTime RecommendedStartYear { get; set; }

        /* Recommended End */
        public TimeSpan RecommendedEndTime { get; set; }
        public DateTime RecommendedEndDate { get; set; }
        public string RecommendedEndDayOfWeek { get; set; }
        public int RecommendedEndDayOfMonth { get; set; }
        public DateTime RecommendedEndWeek { get; set; }
        public DateTime RecommendedEndMonth { get; set; }
        public DateTime RecommendedEndYear { get; set; }
        
        /* Due */
        public TimeSpan DueTime { get; set; }
        public DateTime DueDate { get; set; }
        public string DueDayOfWeek { get; set; }
        public int? DueDayOfMonth { get; set; }
        public DateTime DueWeek { get; set; }
        public DateTime DueMonth { get; set; }
        public DateTime DueYear { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }






        public DateTime StartDateTime
        {
            get
            {
                return new DateTime(this.StartDate.Year, this.StartDate.Month, this.StartDate.Day,
                    this.StartTime.Hours, this.StartTime.Minutes, this.StartTime.Seconds);
            }
        }

        public DateTime EndDateTime
        {
            get
            {
                return new DateTime(this.EndDate.Year, this.EndDate.Month, this.EndDate.Day,
                    this.EndTime.Hours, this.EndTime.Minutes, this.EndTime.Seconds);
            }
        }

        public string StartDateTime_String
        {
            get { return this.StartDateTime.ToString(); }
        }

        public string EndDateTime_String
        {
            get { return this.EndDateTime.ToString(); }
        }
    }
}
