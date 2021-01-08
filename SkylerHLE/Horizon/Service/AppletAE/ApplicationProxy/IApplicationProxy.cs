using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkylerHLE.Horizon.Service.AppletAE.ApplicationProxy
{
    public class IApplicationProxy : ICommandObject
    {
        public Dictionary<ulong, ServiceCall> Calls { get; set; }

        public IApplicationProxy()
        {
            Calls = new Dictionary<ulong, ServiceCall>()
            {
                {0,     GetCommonStateGetter },
                {1,     GetSelfController},
                {2,     GetWindowController},
                {3,     GetAudioController},
                {4,     GetDisplayController },
                {10,    GetProcessWindingController },
                {11,    GetLibraryAppletCreator},
                {20,    GetApplicationFunctions},
                {1000,  GetDebugFunctions}
            };
        }

        public ulong GetCommonStateGetter(CallContext context)
        {
            Helper.Make(context,new ICommonStateGetter());

            return 0;
        }

        public ulong GetSelfController(CallContext context)
        {
            Helper.Make(context,new ISelfController());

            return 0;
        }

        public ulong GetWindowController(CallContext context)
        {
            Helper.Make(context,new IWindowController());

            return 0;
        }

        public ulong GetAudioController(CallContext context)
        {
            Helper.Make(context,new IAudioController());

            return 0;
        }

        public ulong GetDisplayController(CallContext context)
        {
            Helper.Make(context,new IDisplayController());

            return 0;
        }

        public ulong GetProcessWindingController(CallContext context)
        {
            Helper.Make(context,new IProcessWindingController());

            return 0;
        }

        public ulong GetLibraryAppletCreator(CallContext context)
        {
            Helper.Make(context,new ILibraryAppletCreator());

            return 0;
        }

        public ulong GetApplicationFunctions(CallContext context)
        {
            Helper.Make(context,new IApplicationFunctions());

            return 0;
        }

        public ulong GetDebugFunctions(CallContext context)
        {
            Helper.Make(context,new IDebugFunctions());

            return 0;
        }
    }
}
