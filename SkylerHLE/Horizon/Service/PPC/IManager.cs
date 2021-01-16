using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkylerHLE.Horizon.Service.PPC
{
    public class IManager : ICommandObject
    {
        public Dictionary<ulong, ServiceCall> Calls { get; set; } 

        public IManager()
        {
            Calls = new Dictionary<ulong, ServiceCall>()
            {
                {0,OpenSession }
            };
        }

        public ulong OpenSession(CallContext context)
        {
            Helper.Make(context,new ISession());

            return 0;
        }
    }
}
