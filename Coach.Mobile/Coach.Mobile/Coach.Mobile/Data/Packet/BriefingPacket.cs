using Coach.Mobile.Models;
using Coach.Mobile.Models.Briefing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Coach.Mobile.Data.Packet
{
    public class BriefingPacket : CoachPacket, ICoachPacket
    {
        public BriefingPacket()
            : base() { }

        public BriefingPacket(ICoachModel coachModel)
            : base(coachModel) { }

        public BriefingPacket(List<ICoachModel> coachModels)
            : base(coachModels) { }

        public override List<ICoachModel> CoachModels
        {
            get
            {
                return this.CoachModels_Cast.Cast<ICoachModel>().ToList();
            }
            set
            {
                this.CoachModels_Cast = value.Cast<BriefingModel>().ToList();
            }
        }

        //[DataMember]
        public List<BriefingModel> CoachModels_Cast { get; set; }
    }
}
