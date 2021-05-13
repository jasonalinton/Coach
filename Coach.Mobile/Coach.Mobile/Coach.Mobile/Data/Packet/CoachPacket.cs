using Coach.Mobile.Data.Packet.Interface;
using Coach.Mobile.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Coach.Mobile.Data.Packet
{
    //[DataContract]
    public class CoachPacket : ICoachPacket
    {
        public CoachPacket()
        {
            this.CoachModels = new List<ICoachModel>();
        }

        public CoachPacket(ICoachModel iCubeModel)
        {
            this.CoachModels = new List<ICoachModel>() { iCubeModel };
        }

        public CoachPacket(List<ICoachModel> iCubeModels)
        {
            this.CoachModels = iCubeModels;
        }

        //[DataMember]
        public virtual ICoachModel CoachModel => CoachModels.FirstOrDefault();
        public virtual List<ICoachModel> CoachModels { get; set; }

        public static ICoachPacket CreatePacket(ICoachModel model)
        {
            return CreatePacket(new List<ICoachModel> { model });
        }

        public static ICoachPacket CreatePacket(List<ICoachModel> models)
        {
            string modelName = (models.Count > 0) ? models[0].ModelName : "";

            ICoachPacket coachPacket = new CoachPacket();

            if (modelName == "InventoryItemModel")
            {
                coachPacket = new InventoryItemPacket(models);
            }
            else if (modelName == "TypeModel")
            {
                coachPacket = new TypePacket(models);
            }
            else if (modelName == "BriefingModel")
            {
                coachPacket = new BriefingPacket(models);
            }

            return coachPacket;
        }
    }
}
