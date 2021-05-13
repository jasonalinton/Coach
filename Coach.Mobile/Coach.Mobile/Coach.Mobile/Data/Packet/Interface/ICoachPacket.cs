using Coach.Mobile.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Coach.Mobile.Data.Packet
{
    public interface ICoachPacket
    {
        ICoachModel CoachModel { get; }
        List<ICoachModel> CoachModels { get; set; }
    }
}
