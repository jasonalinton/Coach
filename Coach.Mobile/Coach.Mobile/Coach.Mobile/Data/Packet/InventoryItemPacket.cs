using Coach.Mobile.Models;
using Coach.Mobile.Models.InventoryItem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Coach.Mobile.Data.Packet.Interface
{
    public class InventoryItemPacket : CoachPacket, ICoachPacket
    {
        public InventoryItemPacket()
            : base() { }

        public InventoryItemPacket(ICoachModel coachModel)
            : base(coachModel) { }

        public InventoryItemPacket(List<ICoachModel> coachModels)
            : base(coachModels) { }

        public override List<ICoachModel> CoachModels
        {
            get
            {
                return this.CoachModels_Cast.Cast<ICoachModel>().ToList();
            }
            set
            {
                this.CoachModels_Cast = value.Cast<InventoryItemModel>().ToList();
            }
        }

        //[DataMember]
        public List<InventoryItemModel> CoachModels_Cast { get; set; }
    }
}
