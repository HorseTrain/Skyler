using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkylerHLE.Horizon.Service.AppletAE.ApplicationProxy
{
    public class IApplicationFunctions : ICommandObject
    {
        public Dictionary<ulong, ServiceCall> Calls { get; set; } 

        public IApplicationFunctions()
        {
            Calls = new Dictionary<ulong, ServiceCall>()
            {
                {40,  NotifyRunning},
                {66,  InitializeGamePlayRecording },
                {67,  SetGamePlayRecordingState }
            };
        }

        public ulong NotifyRunning(CallContext context)
        {
            context.Writer.Write(1);

            return 0;
        }

        public ulong InitializeGamePlayRecording(CallContext context)
        {
            context.PrintStubbed();

            return 0;
        }

        public ulong SetGamePlayRecordingState(CallContext context)
        {
            context.PrintStubbed();

            return 0;
        }
    }
}
