using Coach.Data.Extension;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Coach.Site.Models.ViewModels.Planner
{
    public class TimelineVM
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="date"></param>
        /// <param name="padding">The number of weeks before and after current week. DEFAULT = 4 Meaning weeks before & 4 week after the date's week</param>
        public TimelineVM(DateTime date, int padding)
        {
            CurrentDate = date.Date;
            Weeks = InitializeWeeks(date, padding);
        }

        public DateTime CurrentDate { get; set; }
        public List<Week> Weeks { get; set; }

        public static List<Week> InitializeWeeks(DateTime date, int padding)
        {
            var weeks = new List<Week>();
            var currentDate = date;

            /* Initialize Weeks */
            date = date.AddDays(-(padding * 7));
            var numberOfWeeks = (padding * 2) + 1;
            for (int i = 0; i < numberOfWeeks; i++)
            {
                weeks.Add(new Week(date, currentDate, i));
                date = date.AddDays(7);
            }

            return weeks;
        }
        // TODO: MOVE THIS TO A PARENT PLANNER VM
        public static List<Day> GetDays(DateTime date, int padding) => InitializeWeeks(date, padding).SelectMany(x => x.Days).ToList();
    }

    public class Week
    {
        public Week(DateTime date, DateTime currentDate, int index)
        {
            Index = index;
            date = date.StartOfWeek();

            Days = new List<Day>();
            Days.Add(new Day(date, currentDate));

            for (int i = 1; i < 7; i++)
            {
                date = date.AddDays(1);
                Days.Add(new Day(date, currentDate));
            }
        }

        public int Index { get; set; }
        public List<Day> Days { get; set; }

        public override string ToString()
        {
            return $"Week of {Days[0]}";
        }
    }

    public class Day
    {
        public Day(DateTime date, DateTime currentDate)
        {
            Date = date.Date;
            CurrentDate = currentDate.Date;
        }

        public DateTime Date { get; set; }
        /// <summary>
        /// Used to determine the era
        /// </summary>
        public DateTime CurrentDate { get; set; }
        public string Month => Date.GetMonth();
        public int DayOfMonth => Date.Day;
        public string DayOfWeek => Date.DayOfWeek.ToString();
        public string DateString => Date.ToShortDateString();
        public int PercentComplete => 63;
        public string Era
        {
            get
            {
                if (Date < CurrentDate)
                    return "past";
                else if (Date == CurrentDate)
                    return "present";
                else
                    return "future";
            }
        }
        public string SelectionClass
        {
            get
            {
                if (Era == "present")
                    return "selected";
                else
                    return String.Empty;
            }
        }

        public override string ToString()
        {
            return $"{Date.ToShortDateString()}";
        }
    }
}