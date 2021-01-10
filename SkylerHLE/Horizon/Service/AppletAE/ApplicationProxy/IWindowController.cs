using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkylerHLE.Horizon.Service.AppletAE.ApplicationProxy
{
    public class IWindowController : ICommandObject
    {
        public Dictionary<ulong, ServiceCall> Calls { get; set; }

        public IWindowController()
        {
            Calls = new Dictionary<ulong, ServiceCall>()
            {
                {1,     GetAppletResourceUserId },
                {10,    AcquireForegroundRights },
            };
        }

        public ulong GetAppletResourceUserId(CallContext context)
        {
            context.PrintStubbed();

            context.Writer.Write(0L);

            return 0;
        }

        public ulong AcquireForegroundRights(CallContext context)
        {
            context.PrintStubbed();

            return 0;
        }
    }
}
