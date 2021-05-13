using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Coach.Model.Planner.Time.Types;

namespace Coach.Model.Planner.Time
{
    public class Time
    {
        private int _idMoment;
        private int _idEndPoint;
        private int _idType;

        public Time(TimeTypes type, Endpoints endPoint, Moments moment)
        {
            Type = type;
            EndPoint = endPoint;
            Moment = moment;
        }

        public DateTime DateTime { get; set; }
        public string DateTimeString => DateTime.ToLongDateString();
        public TimeTypes Type { get; set; }
        public Endpoints EndPoint { get; set; }
        public Moments Moment { get; set; }

        public int idType
        {
            get => _idType;
            set
            {
                Type = (TimeTypes)value;
                _idType = value;
            }
        }
        public int idEndPoint
        {
            get => _idEndPoint;
            set
            {
                EndPoint = (Endpoints)value;
                _idEndPoint = value;
            }
        }
        public int idMoment
        {
            get => _idMoment;
            set
            {
                Moment = (Moments)value;
                _idMoment = value;
            }
        }
    }

    public class Repeat
    {
        private int _idTimeframe;
        private int _idMoment;

        public Time Start { get; set; }
        public Time End { get; set; }
        public List<Time> Starts => new List<Time>();
        public List<Time> Ends => new List<Time>();
        public Timeframes Timeframe { get; set; }
        public Moments Moment { get; set; }
        public int idTimeframe
        {
            get => _idTimeframe;
            set
            {
                Timeframe = (Timeframes)value;
                _idTimeframe = value;
            }
        }
        public int idMoment
        {
            get => _idMoment;
            set
            {
                Moment = (Moments)value;
                _idMoment = value;
            }
        }
        public int Interval { get; set; }
        public int Frequency { get; set; }
        public string PointInTime { get; set; } // This could either be a string for Day of the Week or a string for the day of the month
        public int PointInTimeIndex { get; set; }
    }

    public class Types
    {
        public enum TimeTypes
        {
            Actual = 80,
            Recommended = 81,
            Due = 82
        }

        public enum Endpoints
        {
            Start = 84,
            End = 85
        }

        public enum Timeframes
        {
            Millisecond = 39,
            Second = 40,
            Minute = 41,
            Hour = 42,
            Day = 43,
            Workday = 44,
            Weekday = 45,
            Weekenday = 46,
            Weekend = 47,
            Week = 48,
            Month = 49,
            Season = 50,
            Quarter = 51,
            Trimester = 52,
            Bimester = 53,
            Semester = 54,
            Year = 55,
            Milestone = 56,
            Life = 57
        }

        public enum Moments
        {
            Date = 87,
            Time = 88,
            DateTime = 89,
            PointInTime = 90,
            PointInTimeIndex = 91
        }

        public enum DayOfWeek
        {
            Sunday = 0,
            Monday = 1,
            Tuesday = 2,
            Wednesday = 3,
            Thursday = 4,
            Friday = 5,
            Saturday = 6
        }
    }
}
