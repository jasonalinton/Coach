using Coach.Mobile.Models;
using Coach.Mobile.Models.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Coach.Mobile.Data.Packet
{
    public class TypePacket : CoachPacket, ICoachPacket
    {
        public TypePacket()
            : base() { }

        public TypePacket(ICoachModel coachModel)
            : base(coachModel) { }

        public TypePacket(List<ICoachModel> coachModels)
            : base(coachModels) { }

        public override List<ICoachModel> CoachModels
        {
            get
            {
                return this.CoachModels_Cast.Cast<ICoachModel>().ToList();
            }
            set
            {
                this.CoachModels_Cast = value.Cast<TypeModel>().ToList();
            }
        }

        //[DataMember]
        public List<TypeModel> CoachModels_Cast { get; set; }
    }
}
