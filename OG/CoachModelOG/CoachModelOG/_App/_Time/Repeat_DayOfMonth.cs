using System;
using System.Collections.Generic;
using System.Text;

namespace CoachModel._App._Time
{
    public class Repeat_DayOfMonth
    {
        public int idRepeat_DayOfMonth { get; set; }
        public int idRepeat { get; set; }

        public int DayOfMonth { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public override string ToString()
        {
            return $"Repeat_DayOfMonth: {DayOfMonth}";
        }
    }
}
