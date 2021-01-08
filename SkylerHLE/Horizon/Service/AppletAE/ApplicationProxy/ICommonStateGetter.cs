using SkylerCommon.Debugging;
using SkylerHLE.Horizon.IPC;
using SkylerHLE.Horizon.Service.Sessions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkylerHLE.Horizon.Service.AppletAE.ApplicationProxy
{
    public class ICommonStateGetter : ICommandObject
    {
        public Dictionary<ulong, ServiceCall> Calls { get; set; }

        public ICommonStateGetter()
        {
            Calls = new Dictionary<ulong, ServiceCall>()
            {
                {0, GetEventHandle },
                {1, ReceiveMessage }
            };
        }

        public ulong GetEventHandle(CallContext context)
        {
            //TODO:

            Debug.LogError("",true);

            return 0;
        }

        public static ulong ReceiveMessage(CallContext context)
        {
            //TODO:

            Debug.LogError("", true);

            return 0;
        }
    }
}
