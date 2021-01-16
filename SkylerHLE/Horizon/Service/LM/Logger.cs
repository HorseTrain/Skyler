using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkylerHLE.Horizon.Service.LM
{
    public class Logger : ICommandObject
    {
        public Dictionary<ulong, ServiceCall> Calls { get; set; } = new Dictionary<ulong, ServiceCall>();

        public Logger()
        {
            Calls = new Dictionary<ulong, ServiceCall>()
            {
                {0, Call }
            };
        }

        public ulong Call(CallContext context)
        {
            context.PrintStubbed();

            return 0;
        }
    }
}
