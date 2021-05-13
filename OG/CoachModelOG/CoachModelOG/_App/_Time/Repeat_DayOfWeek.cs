using System;
using System.Collections.Generic;
using System.Text;

namespace CoachModel._App._Time
{
    public class Repeat_DayOfWeek
    {
        public int idRepeat_DayOfWeek { get; set; }
        public int idRepeat { get; set; }

        public string DayOfWeek { get; set; }
        public string Position { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public override string ToString()
        {
            return $"Repeat_DayOfWeek: {Position} {DayOfWeek}";
        }
    }
}
