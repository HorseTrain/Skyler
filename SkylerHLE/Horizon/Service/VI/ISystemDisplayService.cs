using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkylerHLE.Horizon.Service.VI
{
    public class ISystemDisplayService : ICommandObject
    {
        public Dictionary<ulong, ServiceCall> Calls { get; set; } = new Dictionary<ulong, ServiceCall>()
        {
            { 2205, SetLayerZ }
        };
        public static ulong SetLayerZ(CallContext context)
        {
            //TODO:

            return 0;
        }
    }
}
