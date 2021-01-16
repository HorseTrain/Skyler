using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkylerHLE.Horizon.Service.ACC
{
    public class IManagerForApplication : ICommandObject
    {
        public Dictionary<ulong, ServiceCall> Calls { get; set; }

        public IManagerForApplication()
        {
            Calls = new Dictionary<ulong, ServiceCall>()
            {
                { 0, CheckAvailability },
                { 1, GetAccountId      }
            };
        }

        public ulong CheckAvailability(CallContext context)
        {
            context.PrintStubbed();

            return 0;
        }

        public ulong GetAccountId(CallContext context)
        {
            context.PrintStubbed();

            context.Writer.Write(0xcafeL);

            return 0;
        }
    }
}
