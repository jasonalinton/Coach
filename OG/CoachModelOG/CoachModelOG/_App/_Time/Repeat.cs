using System;
using System.Collections.Generic;
using System.Text;

namespace CoachModel._App._Time
{
    public class Repeat
    {
        public Repeat()
        {
            this.Repeat_DayOfWeeks = new List<Repeat_DayOfWeek>();
            this.Repeat_DayOfMonths = new List<Repeat_DayOfMonth>();
            this.Repeat_Months = new List<Repeat_Month>();

            this.TimeframeClass = new Timeframe();
        }

        public int idRepeat { get; set; }
        public int idTimeframe { get; set; }

        public int interval { get; set; }
        public int frequency { get; set; }

        public string Repeat_DayOfWeekIDs_Concat { get; set; }
        public string Repeat_DayOfMonthIDs_Concat { get; set; }
        public string Repeat_MonthIDs_Concat { get; set; }

        public List<Repeat_DayOfWeek> Repeat_DayOfWeeks { get; set; }
        public List<Repeat_DayOfMonth> Repeat_DayOfMonths { get; set; }
        public List<Repeat_Month> Repeat_Months { get; set; }

        public List<int> Repeat_DayOfWeekIDs
        {
            get
            {
                if (this.Repeat_DayOfWeekIDs_Concat != null)
                {
                    var stringList = new List<string>(this.Repeat_DayOfWeekIDs_Concat.Split(','));
                    return stringList.ConvertAll(int.Parse);
                }
                else
                {
                    return new List<int>();
                }
            }
        }
        public List<int> Repeat_DayOfMonthIDs
        {
            get
            {
                if (this.Repeat_DayOfMonthIDs_Concat != null)
                {
                    var stringList = new List<string>(this.Repeat_DayOfMonthIDs_Concat.Split(','));
                    return stringList.ConvertAll(int.Parse);
                }
                else
                {
                    return new List<int>();
                }
            }
        }
        public List<int> Repeat_MonthIDs
        {
            get
            {
                if (this.Repeat_MonthIDs_Concat != null)
                {
                    var stringList = new List<string>(this.Repeat_MonthIDs_Concat.Split(','));
                    return stringList.ConvertAll(int.Parse);
                }
                else
                {
                    return new List<int>();
                }
            }
        }

        public Timeframe TimeframeClass { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public override string ToString()
        {
            return $"Repeat: ";
        }
    }
}
