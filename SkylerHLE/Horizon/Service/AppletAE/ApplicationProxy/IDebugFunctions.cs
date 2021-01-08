using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkylerHLE.Horizon.Service.AppletAE.ApplicationProxy
{
    public class IDebugFunctions : ICommandObject
    {
        public Dictionary<ulong, ServiceCall> Calls { get; set; }

        public IDebugFunctions()
        {
            Calls = new Dictionary<ulong, ServiceCall>()
            {

            };
        }
    }
}
