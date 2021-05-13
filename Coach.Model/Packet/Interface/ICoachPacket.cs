using Coach.Model.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coach.Model.Packet.Interface
{
    public interface ICoachPacket
    {
        ICoachModel CoachModel { get; }
        List<ICoachModel> CoachModels { get; set; }
    }
}
