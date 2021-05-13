using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coach.Model.Items
{
    public class EventTaskModel
    {
        public int ID { get; set; }
        public string Text { get; set; }
        public sbyte? IsComplete { get; set; }
        public sbyte? IsAttempted { get; set; }
        public DateTime StartDateTime { get; set; }
        public DateTime EndDateTime { get; set; }
        public DateTime DateTimeCompleted { get; set; }
        public string idGoogleTask { get; set; }
        public string idGoogleList { get; set; }
    }
}
