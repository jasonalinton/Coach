using Coach.Data.EFExtention.Model.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Coach.Model.Planner.Time.Types;

namespace Coach.Data.Model
{
    public partial class time : ICoachDevModel
    {
        public string TableName => "time";
        public string DisplayName => "Time";
        public string text
        {
            get => $"{((Moments)idMoment).ToString()} {((Endpoints)idEndpoint).ToString()}s at {MomentString}";
            set { }
        }

        public string MomentString
        {
            get
            {
                if (idMoment == (int)Moments.Date)
                    return datetime.ToShortDateString();
                else if (idMoment == (int)Moments.Time)
                    return datetime.ToShortTimeString();
                else
                    return datetime.ToLongDateString();
            }
        }

        public override string ToString() => $"{DisplayName}: {text}";
    }
}