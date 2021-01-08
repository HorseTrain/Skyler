using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkylerHLE.Horizon.Service.VI
{
    public class IManagerDisplayService : ICommandObject
    {
        public Dictionary<ulong, ServiceCall> Calls { get; set; } = new Dictionary<ulong, ServiceCall>()
        {
            {2010,          CreateManagedLayer},
            {6000,          AddLayerToStack},
        };

        public static ulong CreateManagedLayer(CallContext context)
        {
            context.Writer.Write(0UL);

            return 0;
        }

        public static ulong AddLayerToStack(CallContext context)
        {
            return 0;
        }
    }
}
